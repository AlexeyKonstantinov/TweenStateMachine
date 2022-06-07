using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TweensStateMachine.EditorScripts
{
    public static class EditorHelpers
    {
        public static void AddItemString(this GenericMenu menu, string item, GenericMenu.MenuFunction callback)
        {
            menu.AddItem(new GUIContent(item), false, callback);
        }

        public static List<T> GetEditors<T>() where T : Editor
        {
            var editors = Resources.FindObjectsOfTypeAll<T>().ToList();
            return editors;
        }
    }
}