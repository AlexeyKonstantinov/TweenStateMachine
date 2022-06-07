using TweensStateMachine.Runtime.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TweensStateMachine.EditorScripts
{
    public class TweenStateMachineWindow : EditorWindow
    {
        public VisualTreeAsset visualTree;

        private SerializedObject _serializedObject;
    
        [MenuItem("Tools/Tween State Editor")]
        public static TweenStateMachineWindow Open()
        {
            var wnd = GetWindow<TweenStateMachineWindow>();
            wnd.titleContent = new GUIContent("Tween State Machine Inspector");
            return wnd;
        }

        public void Init(TweenStateMachine target)
        {
            rootVisualElement.Clear();
            var wnd = visualTree.CloneTree();
            _serializedObject = new SerializedObject(target);
            var statesContainer = wnd.Q("unity-content-container");
            var states = _serializedObject.FindProperty("states");
            for (int i = 0; i < states.arraySize; i++)
            {
                var stateProperty = states.GetArrayElementAtIndex(i);
                statesContainer.Add(new PropertyField(stateProperty));
            }

            wnd.Bind(_serializedObject);
            rootVisualElement.Add(wnd);
            rootVisualElement.Bind(_serializedObject);
        }
        
        public void UpdateAndRebind()
        {
            _serializedObject.Update();
            rootVisualElement.Clear();
            var wnd = visualTree.CloneTree();
            var statesContainer = wnd.Q("unity-content-container");
            var states = _serializedObject.FindProperty("states");
            for (int i = 0; i < states.arraySize; i++)
            {
                var stateProperty = states.GetArrayElementAtIndex(i);
                statesContainer.Add(new PropertyField(stateProperty));
            }
            wnd.Bind(_serializedObject);
            rootVisualElement.Add(wnd);
            rootVisualElement.Bind(_serializedObject);
        }

        public void CreateGUI()
        {
            
        }
    }
}