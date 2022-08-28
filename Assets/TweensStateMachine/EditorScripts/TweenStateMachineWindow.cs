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
        private TabView _tabView;
        private SerializedObject _serializedObject;
    
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
            wnd.style.flexGrow = 1;
            _serializedObject = new SerializedObject(target);
            var root = wnd.Q("root");
            var statesProp = _serializedObject.FindProperty("states");

            _tabView = new TabView();
            _tabView.OnTabClick += Rebind;
            root.Add(_tabView);

            for (int i = 0; i < statesProp.arraySize; i++)
            {
                var stateProp = statesProp.GetArrayElementAtIndex(i);
                var stateElement = new StateElement();
                stateElement.Init(stateProp);
                _tabView.AddTab($"{stateProp.FindPropertyRelative("stateName").stringValue}", stateElement);
            }
            
            rootVisualElement.Add(wnd);
            // rootVisualElement.Bind(_serializedObject);
        }

        private void OnDestroy()
        {
            _tabView.OnTabClick -= Rebind;
        }

        private void Rebind()
        {
            rootVisualElement.Bind(_serializedObject);
        }

        public void CreateGUI()
        {
            
        }
    }
}