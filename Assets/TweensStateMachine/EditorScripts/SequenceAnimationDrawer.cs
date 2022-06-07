using System;
using TweensStateMachine.Runtime.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TweensStateMachine.EditorScripts
{
    [CustomPropertyDrawer(typeof(SequenceAnimation))]
    public class SequenceAnimationDrawer : PropertyDrawer
    {
        private Button deleteButton;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var ussStyles = Resources.Load<StyleSheet>("UssStyles");
            var vsTree = Resources.Load<VisualTreeAsset>("SequenceAnimationDrawerUXML");
            var root = new VisualElement();
            root.styleSheets.Add(ussStyles);
            vsTree.CloneTree(root);

            var addButton = root.Q<VisualElement>("addButton");
            addButton.RegisterCallback<ClickEvent, SerializedProperty>(OpenAddMenu, property);

            var sequenceAnimation = (SequenceAnimation) SerializedFieldHelper.GetTargetObjectOfProperty(property);
            var animationsContainer = root.Q<VisualElement>("animationsContainer");
            var animList = property.FindPropertyRelative("animations");
            for (int i = 0; i < animList.arraySize; i++)
            {
                var animProp = animList.GetArrayElementAtIndex(i);
                var animation = (AnimationBase) SerializedFieldHelper.GetTargetObjectOfProperty(animProp);
                
                var container = new VisualElement();
                container.AddToClassList("animation-container");
                container.Add(new PropertyField(animProp));
                deleteButton = new Button(() =>
                {
                    sequenceAnimation.RemoveAnimation(animation);
                    foreach (var editor in Resources.FindObjectsOfTypeAll<TweenStateMachineWindow>())
                    {
                        editor.UpdateAndRebind();
                    }
                }) {text = "-"};
                deleteButton.AddToClassList("delete-button");
                container.Add(deleteButton);
                animationsContainer.Add(container);
            }
            
            return root;
        }

        private void OpenAddMenu(ClickEvent evt, SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItemString("Sequence", () =>
            {
                var sequenceAnimation = (SequenceAnimation) SerializedFieldHelper.GetTargetObjectOfProperty(property);
                var anim = (AnimationBase) Activator.CreateInstance(typeof(SequenceAnimation));
                sequenceAnimation.AddAnimation(anim);
                foreach (var editor in Resources.FindObjectsOfTypeAll<TweenStateMachineWindow>())
                {
                    editor.UpdateAndRebind();
                }
            });
            menu.AddSeparator("");
            foreach (var type in TypeCache.GetTypesDerivedFrom<TweenAnimation>())
            {
                var name = type.FullName.Replace("TweensStateMachine.Animations.", "").Replace(".", "/");
                menu.AddItemString(name, () =>
                {
                    var sequenceAnimation = (SequenceAnimation) SerializedFieldHelper.GetTargetObjectOfProperty(property);
                    var anim = (AnimationBase) Activator.CreateInstance(type);
                    sequenceAnimation.AddAnimation(anim);
                    foreach (var editor in Resources.FindObjectsOfTypeAll<TweenStateMachineWindow>())
                    {
                        editor.UpdateAndRebind();
                    }
                });
            }
            menu.ShowAsContext();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}