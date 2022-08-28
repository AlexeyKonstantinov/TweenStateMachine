using TweensStateMachine.Animations.Move;
using UnityEditor;
using UnityEngine.UIElements;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.EditorScripts
{
    [CustomEditor(typeof(TweenStateMachine))]
    public class TweenStateMachineEditor : Editor
    {
        public VisualTreeAsset visualTreeAsset;
        private VisualElement _rootVisualElement;

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                var wnd = TweenStateMachineWindow.Open();
                wnd.Init((TweenStateMachine) target);
            }
            
            if (GUILayout.Button("Add State"))
            {
                var tgt = (TweenStateMachine) target;
                tgt.AddState("State_1", new MoveAnimation());
            }
            
            if (GUILayout.Button("Add State 2"))
            {
                var tgt = (TweenStateMachine) target;
                tgt.AddState("State_2", new MoveXAnimation());
            }
            
            if (GUILayout.Button("All serialized fields"))
            {
                string text = "";
                var property = serializedObject.GetIterator();
                property.NextVisible(true);
                text += $"{property.propertyPath} \n \n";
                // var endProperty = property.GetEndProperty();
                while (property.NextVisible(true))
                {
                    if (property.propertyType == SerializedPropertyType.ManagedReference)
                    {
                        text += "Managed ref: ";
                    }
                    text += $"{property.propertyPath} \n \n";
                    // if(SerializedProperty.EqualContents(property, endProperty))
                    //     break;
                }

                Debug.Log(text);
            }
        }

        // public override VisualElement CreateInspectorGUI()
        // {
            // var root = new VisualElement();
            // visualTreeAsset.CloneTree(root);
            // _rootVisualElement = root;
            // return root;
        // }

        public void UpdateAndRebind()
        {
            // serializedObject.Update();
            // _rootVisualElement.Bind(serializedObject);
        }
    }
}