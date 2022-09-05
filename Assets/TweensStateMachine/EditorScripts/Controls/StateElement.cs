using System;
using System.Linq;
using TweensStateMachine.EditorScripts;
using TweensStateMachine.Runtime.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TweensStateMachine
{
    public class StateElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<StateElement, UxmlTraits> {}

        private SerializedProperty _stateProperty;
        private SequenceElement _sequenceElement;
        private SplitView _splitView;
        private VisualElement _leftPane;
        private VisualElement _leftPaneHeader;
        private Button _addSequenceButton;

        public StateElement()
        {
            style.flexGrow = 1;
            style.flexShrink = 1;
            _splitView = new SplitView(0, 200, TwoPaneSplitViewOrientation.Horizontal);
            _sequenceElement = new SequenceElement();
            _splitView.Add(_sequenceElement);
            _leftPane = new VisualElement {name = "Left Pane"};
            _leftPane.AddToClassList("left-pane");
            _leftPaneHeader = new VisualElement {name = "Left Pane Header"};
            _leftPaneHeader.AddToClassList("left-pane-header");
            _leftPaneHeader.Add(new Label("Tweens"));
            _addSequenceButton = new Button {text = "Add Tween"};
            _leftPaneHeader.Add(_addSequenceButton);
            _leftPane.Add(_leftPaneHeader);
            _splitView.Add(_leftPane);
            Add(_splitView);
        }

        public void Init(SerializedProperty stateProperty)
        {
            _stateProperty = stateProperty;
            _addSequenceButton.RegisterCallback<ClickEvent, SerializedProperty>(OpenAddMenu, _stateProperty);
            Rebuild();
        }

        public void UpdateViewWithSerializedData()
        {
            _sequenceElement.UpdateViewWithSerializedData();
        }

        private void Rebuild()
        {
            _sequenceElement.ClearElement();
            _leftPane.Clear();
            _leftPane.Add(_leftPaneHeader);
            
            var animationsProperty = _stateProperty.FindPropertyRelative("sequence.animations");
            for (int i = 0; i < animationsProperty.arraySize; i++)
            {
                var animation = animationsProperty.GetArrayElementAtIndex(i);
                AddAnimation(animation);
            }
            this.Bind(_stateProperty.serializedObject);
        }

        private void AddAnimation(SerializedProperty animationProperty)
        {
            var animDataElementUxml = Resources.Load<VisualTreeAsset>("AnimationDataTemplate");
            var animDataElement = animDataElementUxml.CloneTree();

            var animationNameLabel = animDataElement.Q<Label>("nameLabel");
            var animType = SerializedFieldHelper.GetTargetObjectOfProperty(animationProperty).GetType().ToString();
            animType = animType.Split('.').Last();
            animType = animType.Replace("Animation", "");
            animationNameLabel.text = animType;
            
            var durationField = animDataElement.Q<FloatField>("durationField");
            durationField.bindingPath = animationProperty.propertyPath + ".duration";
            
            var delayField = animDataElement.Q<FloatField>("delayField");
            delayField.bindingPath = animationProperty.propertyPath + ".delay";
            
            var targetProp = animationProperty.FindPropertyRelative("target");
            if (targetProp != null)
            {
                var targetField = animDataElement.Q<ObjectField>("targetProperty");
                targetField.objectType = GetType(targetProp);
                targetField.bindingPath = animationProperty.propertyPath + ".target";
            }

            var valueProperty = animationProperty.FindPropertyRelative("value");
            if (valueProperty != null)
            {
                var valueFieldContainer = animDataElement.Q<VisualElement>("valuePropertyContainer");
                if (valueProperty.propertyType == SerializedPropertyType.Vector3)
                {
                    var propElement = new Vector3Field();
                    valueFieldContainer.Add(propElement);
                    propElement.bindingPath = valueProperty.propertyPath;
                    propElement.Query<Label>().ForEach(label => label.style.color = Color.black);
                }
                else if (valueProperty.propertyType == SerializedPropertyType.Float)
                {
                    var propElement = new FloatField();
                    valueFieldContainer.Add(propElement);
                    propElement.bindingPath = valueProperty.propertyPath;
                    propElement.Query<Label>().ForEach(label => label.style.color = Color.black);
                }
            }

            var getButton = animDataElement.Q<Button>("getButton");
            getButton.RegisterCallback<ClickEvent, SerializedProperty>(GetButtonClick, animationProperty);

            var closeButton = animDataElement.Q<Button>("closeButton");
            closeButton.RegisterCallback<ClickEvent, SerializedProperty>(CloseButtonClick, animationProperty);

            _leftPane.Add(animDataElement);
            
            _sequenceElement.AddTrack(animationProperty);
        }

        private void CloseButtonClick(ClickEvent evt, SerializedProperty animationProperty)
        {
            var animObj = (AnimationBase) SerializedFieldHelper.GetTargetObjectOfProperty(animationProperty);
            var stateObj = (State) SerializedFieldHelper.GetTargetObjectOfProperty(_stateProperty);
            stateObj.sequence.animations.Remove(animObj);
            EditorUtility.SetDirty(_stateProperty.serializedObject.targetObject);
            _stateProperty.serializedObject.Update();
            Rebuild();
        }

        private void GetButtonClick(ClickEvent evt, SerializedProperty animationProperty)
        {
            var obj = SerializedFieldHelper.GetTargetObjectOfProperty(animationProperty);
            if (obj is TweenAnimation tweenAnimation)
            {
                Undo.RecordObject(animationProperty.serializedObject.targetObject, "GET value");
                tweenAnimation.GetValue();
            }
        }

        private static Type GetType(SerializedProperty property)
        {
            var typeName = property.type.Replace("PPtr<$", "").Replace(">", "");
            if (typeName == "Transform")
            {
                return typeof(Transform);
            }

            throw new InvalidOperationException("Wrong property type");
        }
        
        private void OpenAddMenu(ClickEvent evt, SerializedProperty stateProperty)
        {
            var menu = new GenericMenu();
            foreach (var type in TypeCache.GetTypesDerivedFrom<TweenAnimation>())
            {
                var className = type.FullName.Replace("TweensStateMachine.Animations.", "").Replace(".", "/");
                menu.AddItemString(className, () =>
                {
                    var state = (State) SerializedFieldHelper.GetTargetObjectOfProperty(stateProperty);
                    var anim = (AnimationBase) Activator.CreateInstance(type);
                    state.sequence.AddAnimation(anim);
                    EditorUtility.SetDirty(stateProperty.serializedObject.targetObject);
                    stateProperty.serializedObject.Update();
                    Rebuild();
                });
            }
            menu.ShowAsContext();
        }
    }
}