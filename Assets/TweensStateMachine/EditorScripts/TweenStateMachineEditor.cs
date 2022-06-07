using UnityEditor;
using UnityEditor.UIElements;
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