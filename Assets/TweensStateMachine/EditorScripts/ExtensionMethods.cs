using UnityEditor;

namespace TweensStateMachine.EditorScripts
{
    public static class ExtensionMethods
    {
        public static float GetDelayProperty(this SerializedProperty property)
        {
            return property.FindPropertyRelative("delay").floatValue;
        }
        
        public static void SetDelayProperty(this SerializedProperty property, float value)
        {
            property.FindPropertyRelative("delay").floatValue = value;
        }
        
        public static float GetDurationProperty(this SerializedProperty property)
        {
            return property.FindPropertyRelative("duration").floatValue;
        }
        
        public static void SetDurationProperty(this SerializedProperty property, float value)
        {
            property.FindPropertyRelative("duration").floatValue = value;
        }
    }
}