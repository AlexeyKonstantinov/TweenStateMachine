using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TweensStateMachine.EditorScripts
{
    public class TabView : VisualElement
    {
        private Dictionary<string, VisualElement> _tabs;
        private Dictionary<string, ToolbarToggle> _tabToggles;
        private VisualElement _toolbar;

        public event Action OnTabClick;

        public new class UxmlFactory : UxmlFactory<TabView, UxmlTraits>{}

        public sealed override VisualElement contentContainer { get; }

        public TabView()
        {
            _tabs = new Dictionary<string, VisualElement>();
            _tabToggles = new Dictionary<string, ToolbarToggle>();
            _toolbar = new Toolbar();
            _toolbar.AddToClassList("toolbar");
            hierarchy.Add(_toolbar);
            contentContainer = new VisualElement {name = "Content"};
            contentContainer.style.flexGrow = 1;
            hierarchy.Insert(1, contentContainer);
            style.flexGrow = 1;
        }

        public void AddTab(string tabName, VisualElement visualElement)
        {
            if (_tabs.ContainsKey(tabName))
                throw new InvalidOperationException($"Tab with name '{tabName}' already exists.");
            _tabs.Add(tabName, visualElement);
            var toggle = new ToolbarToggle {name = tabName, text = tabName};
            toggle.AddToClassList("toolbar-toggle");
            toggle.RegisterCallback<ClickEvent, string>(ClickTabToggle, tabName);
            _toolbar.Add(toggle);
            _tabToggles.Add(tabName, toggle);
        }

        public ToolbarToggle GetToggle(string tabName)
        {
            return _tabToggles[tabName];
        }

        private void ClickTabToggle(ClickEvent evt, string tabName)
        {
            foreach (var tabToggle in _tabToggles.Values) {
                tabToggle.SetValueWithoutNotify(false);
            }
            _tabToggles[tabName].SetValueWithoutNotify(true);
            
            contentContainer.Clear();
            contentContainer.Add(_tabs[tabName]);
            OnTabClick?.Invoke();
        }
    }
}