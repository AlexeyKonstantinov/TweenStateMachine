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
        private TSMAnimation _target;
    
        public static TweenStateMachineWindow Open()
        {
            var wnd = GetWindow<TweenStateMachineWindow>();
            wnd.titleContent = new GUIContent("Tween State Machine Inspector");
            return wnd;
        }

        public void Init(TSMAnimation target)
        {
            _target = target;
            _serializedObject = new SerializedObject(target);
            Rebuild();
        }

        private void Rebuild()
        {
            rootVisualElement.Clear();
            var wnd = visualTree.CloneTree();
            wnd.style.flexGrow = 1;
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
                var stateName = stateProp.FindPropertyRelative("stateName").stringValue;
                _tabView.AddTab($"{stateName}", stateElement);
                _tabView.GetToggle(stateName).RegisterCallback<MouseDownEvent, string>(TabviewToggleClicked, stateName);
            }
            
            rootVisualElement.Add(wnd);
        }

        private void TabviewToggleClicked(MouseDownEvent evt, string stateName)
        {
            Debug.Log(evt.pressedButtons);
            if (evt.pressedButtons != 2) {
                return;
            }
            
            var menu = new GenericMenu();
            menu.AddItemString("Delete", () =>
            {
                var index = _target.states.FindIndex(x => x.stateName == stateName);
                _target.states.RemoveAt(index);
                EditorUtility.SetDirty(_target);
                _serializedObject.Update();
                Rebuild();
            });
            menu.ShowAsContext();
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