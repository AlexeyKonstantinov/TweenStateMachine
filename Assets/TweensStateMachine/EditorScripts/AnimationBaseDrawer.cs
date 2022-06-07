using TweensStateMachine.Runtime.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TweensStateMachine.EditorScripts
{
    [CustomPropertyDrawer(typeof(AnimationBase))]
    public class AnimationBaseDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var vsTree = Resources.Load<VisualTreeAsset>("AnimationBaseDrawerUXML");
            vsTree.CloneTree(root);
            var header = root.Q<Label>("animationName");
            header.text = property.type.Replace("managedReference<", "").Replace(">", "");
            
            var animationsContainer = root.Q<VisualElement>("propertiesContainer");
            var endProperty = property.GetEndProperty();
            property.NextVisible(true);
            while (property.NextVisible(false))
            {
                if(SerializedProperty.EqualContents(property, endProperty))
                    break;
                var prop = new PropertyField(property);
                prop.AddToClassList("thin-property");
                animationsContainer.Add(prop);
            }
            
            // Strange error pops up if you don't reset property after iterating, because sth is trying to
            // iterate over it again. (Sth to do with editor binding UIElements in inspector, I guess)
            property.Reset();
            
            return root;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
    
    // [CustomPropertyDrawer(typeof(MoveAnimation))]
    public class MoveAnimationDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            root.Add(new Label("MoveAnimation"));
            return root;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}