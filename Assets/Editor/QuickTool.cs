using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class QuickTool : EditorWindow
{
    [MenuItem("QuickTool/Open _%#T")]
    public static void ShowWindow()
    {
        var window = GetWindow<QuickTool>();
        window.titleContent = new GUIContent("QuickTool");
        window.minSize = new Vector2(280, 50);
    }

    public void CreateGUI()
    {
        var root = rootVisualElement;
        root.styleSheets.Add(Resources.Load<StyleSheet>("QuickTool_Style"));

        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("QuickTool_Main");
        quickToolVisualTree.CloneTree(root);

        var toolButtons = root.Query(className: "quicktool-button"); 
        toolButtons.ForEach(SetupButton);
    }

    private void SetupButton(VisualElement button)
    {
        var buttonIcon = button.Q(className: "quicktool-button-icon");
        var iconPath = "Icons/" + button.parent.name + "_name";
        var iconAsset = Resources.Load<Texture2D>(iconPath);
        buttonIcon.style.backgroundImage = iconAsset;
        button.RegisterCallback<PointerUpEvent, string>(CreateObject, button.parent.name);
        button.tooltip = button.parent.name;
    }

    private void CreateObject(PointerUpEvent evt, string primitiveTypeName)
    {
        var pt = (PrimitiveType) Enum.Parse(typeof(PrimitiveType), primitiveTypeName, true);
        var go = ObjectFactory.CreatePrimitive(pt);
        go.transform.position = Vector3.zero;
    }
}