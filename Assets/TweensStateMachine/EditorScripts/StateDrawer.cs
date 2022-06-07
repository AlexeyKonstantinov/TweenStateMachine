using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TweensStateMachine.EditorScripts
{
    // [CustomPropertyDrawer(typeof(State))]
    public class StateDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            var vsTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/TweensStateMachine/Editor/StateDrawerUXML.uxml");
            vsTree.CloneTree(root);
            
            // var popup = new UnityEngine.UIElements.PopupWindow();
            // popup.Add(new Label("State Drawer"));
            // popup.Add(new PropertyField(property.FindPropertyRelative("StateName")));
            // popup.Add(new PropertyField(property.FindPropertyRelative("sequence")));

            // root.Add(popup);
            return root;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}