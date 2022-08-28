using System.Collections.Generic;

namespace UnityEngine.UIElements
{
  public class CustomTwoPaneSplitView : VisualElement
  {
    private static readonly string s_UssClassName = "unity-two-pane-split-view";
    private static readonly string s_ContentContainerClassName = "unity-two-pane-split-view__content-container";
    private static readonly string s_HandleDragLineClassName = "unity-two-pane-split-view__dragline";
    private static readonly string s_HandleDragLineVerticalClassName = CustomTwoPaneSplitView.s_HandleDragLineClassName + "--vertical";
    private static readonly string s_HandleDragLineHorizontalClassName = CustomTwoPaneSplitView.s_HandleDragLineClassName + "--horizontal";
    private static readonly string s_HandleDragLineAnchorClassName = "unity-two-pane-split-view__dragline-anchor";
    private static readonly string s_HandleDragLineAnchorVerticalClassName = CustomTwoPaneSplitView.s_HandleDragLineAnchorClassName + "--vertical";
    private static readonly string s_HandleDragLineAnchorHorizontalClassName = CustomTwoPaneSplitView.s_HandleDragLineAnchorClassName + "--horizontal";
    private static readonly string s_VerticalClassName = "unity-two-pane-split-view--vertical";
    private static readonly string s_HorizontalClassName = "unity-two-pane-split-view--horizontal";
    private VisualElement m_LeftPane;
    private VisualElement m_RightPane;
    private VisualElement m_FixedPane;
    private VisualElement m_FlexedPane;
    private VisualElement m_DragLine;
    private VisualElement m_DragLineAnchor;
    private bool m_CollapseMode;
    private VisualElement m_Content;
    private TwoPaneSplitViewOrientation m_Orientation;
    private int m_FixedPaneIndex;
    private float m_FixedPaneInitialDimension;
    internal CustomTwoPaneSplitViewResizer m_Resizer;

    public VisualElement fixedPane => this.m_FixedPane;

    public VisualElement flexedPane => this.m_FlexedPane;

    public int fixedPaneIndex
    {
      get => this.m_FixedPaneIndex;
      set
      {
        if (value == this.m_FixedPaneIndex)
          return;
        this.Init(value, this.m_FixedPaneInitialDimension, this.m_Orientation);
      }
    }

    public float fixedPaneInitialDimension
    {
      get => this.m_FixedPaneInitialDimension;
      set
      {
        if ((double) value == (double) this.m_FixedPaneInitialDimension)
          return;
        this.Init(this.m_FixedPaneIndex, value, this.m_Orientation);
      }
    }

    public TwoPaneSplitViewOrientation orientation
    {
      get => this.m_Orientation;
      set
      {
        if (value == this.m_Orientation)
          return;
        this.Init(this.m_FixedPaneIndex, this.m_FixedPaneInitialDimension, value);
      }
    }

    public CustomTwoPaneSplitView()
    {
      this.AddToClassList(CustomTwoPaneSplitView.s_UssClassName);
      this.m_Content = new VisualElement();
      this.m_Content.name = "unity-content-container";
      this.m_Content.AddToClassList(CustomTwoPaneSplitView.s_ContentContainerClassName);
      this.hierarchy.Add(this.m_Content);
      this.m_DragLineAnchor = new VisualElement();
      this.m_DragLineAnchor.name = "unity-dragline-anchor";
      this.m_DragLineAnchor.AddToClassList(CustomTwoPaneSplitView.s_HandleDragLineAnchorClassName);
      this.hierarchy.Add(this.m_DragLineAnchor);
      this.m_DragLine = new VisualElement();
      this.m_DragLine.name = "unity-dragline";
      this.m_DragLine.AddToClassList(CustomTwoPaneSplitView.s_HandleDragLineClassName);
      this.m_DragLineAnchor.Add(this.m_DragLine);
    }

    public CustomTwoPaneSplitView(
      int fixedPaneIndex,
      float fixedPaneStartDimension,
      TwoPaneSplitViewOrientation orientation)
      : this()
    {
      this.Init(fixedPaneIndex, fixedPaneStartDimension, orientation);
    }

    public void CollapseChild(int index)
    {
      if (this.m_LeftPane == null)
        return;
      this.m_DragLine.style.display = (StyleEnum<DisplayStyle>) DisplayStyle.None;
      this.m_DragLineAnchor.style.display = (StyleEnum<DisplayStyle>) DisplayStyle.None;
      if (index == 0)
      {
        this.m_RightPane.style.width = (StyleLength) StyleKeyword.Initial;
        this.m_RightPane.style.height = (StyleLength) StyleKeyword.Initial;
        this.m_RightPane.style.flexGrow = (StyleFloat) 1f;
        this.m_LeftPane.style.display = (StyleEnum<DisplayStyle>) DisplayStyle.None;
      }
      else
      {
        this.m_LeftPane.style.width = (StyleLength) StyleKeyword.Initial;
        this.m_LeftPane.style.height = (StyleLength) StyleKeyword.Initial;
        this.m_LeftPane.style.flexGrow = (StyleFloat) 1f;
        this.m_RightPane.style.display = (StyleEnum<DisplayStyle>) DisplayStyle.None;
      }
      this.m_CollapseMode = true;
    }

    public void UnCollapse()
    {
      if (this.m_LeftPane == null)
        return;
      this.m_LeftPane.style.display = (StyleEnum<DisplayStyle>) DisplayStyle.Flex;
      this.m_RightPane.style.display = (StyleEnum<DisplayStyle>) DisplayStyle.Flex;
      this.m_DragLine.style.display = (StyleEnum<DisplayStyle>) DisplayStyle.Flex;
      this.m_DragLineAnchor.style.display = (StyleEnum<DisplayStyle>) DisplayStyle.Flex;
      this.m_LeftPane.style.flexGrow = (StyleFloat) 0.0f;
      this.m_RightPane.style.flexGrow = (StyleFloat) 0.0f;
      this.m_CollapseMode = false;
      this.Init(this.m_FixedPaneIndex, this.m_FixedPaneInitialDimension, this.m_Orientation);
    }

    internal void Init(
      int fixedPaneIndex,
      float fixedPaneInitialDimension,
      TwoPaneSplitViewOrientation orientation)
    {
      this.m_Orientation = orientation;
      this.m_FixedPaneIndex = fixedPaneIndex;
      this.m_FixedPaneInitialDimension = fixedPaneInitialDimension;
      this.m_Content.RemoveFromClassList(CustomTwoPaneSplitView.s_HorizontalClassName);
      this.m_Content.RemoveFromClassList(CustomTwoPaneSplitView.s_VerticalClassName);
      if (this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
        this.m_Content.AddToClassList(CustomTwoPaneSplitView.s_HorizontalClassName);
      else
        this.m_Content.AddToClassList(CustomTwoPaneSplitView.s_VerticalClassName);
      this.m_DragLineAnchor.RemoveFromClassList(CustomTwoPaneSplitView.s_HandleDragLineAnchorHorizontalClassName);
      this.m_DragLineAnchor.RemoveFromClassList(CustomTwoPaneSplitView.s_HandleDragLineAnchorVerticalClassName);
      if (this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
        this.m_DragLineAnchor.AddToClassList(CustomTwoPaneSplitView.s_HandleDragLineAnchorHorizontalClassName);
      else
        this.m_DragLineAnchor.AddToClassList(CustomTwoPaneSplitView.s_HandleDragLineAnchorVerticalClassName);
      this.m_DragLine.RemoveFromClassList(CustomTwoPaneSplitView.s_HandleDragLineHorizontalClassName);
      this.m_DragLine.RemoveFromClassList(CustomTwoPaneSplitView.s_HandleDragLineVerticalClassName);
      if (this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
        this.m_DragLine.AddToClassList(CustomTwoPaneSplitView.s_HandleDragLineHorizontalClassName);
      else
        this.m_DragLine.AddToClassList(CustomTwoPaneSplitView.s_HandleDragLineVerticalClassName);
      if (this.m_Resizer != null)
      {
        this.m_DragLineAnchor.RemoveManipulator((IManipulator) this.m_Resizer);
        this.m_Resizer = (CustomTwoPaneSplitViewResizer) null;
      }
      if (this.m_Content.childCount != 2)
        this.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnPostDisplaySetup));
      else
        this.PostDisplaySetup();
    }

    private void OnPostDisplaySetup(GeometryChangedEvent evt)
    {
      if (this.m_Content.childCount != 2)
      {
        Debug.LogError((object) "TwoPaneSplitView needs exactly 2 chilren.");
      }
      else
      {
        this.PostDisplaySetup();
        this.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnPostDisplaySetup));
        this.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnSizeChange));
      }
    }

    private void PostDisplaySetup()
    {
      if (this.m_Content.childCount != 2)
      {
        Debug.LogError((object) "TwoPaneSplitView needs exactly 2 children.");
      }
      else
      {
        this.m_LeftPane = this.m_Content[1];
        if (this.m_FixedPaneIndex == 0)
          this.m_FixedPane = this.m_LeftPane;
        else
          this.m_FlexedPane = this.m_LeftPane;
        this.m_RightPane = this.m_Content[0];
        if (this.m_FixedPaneIndex == 1)
          this.m_FixedPane = this.m_RightPane;
        else
          this.m_FlexedPane = this.m_RightPane;
        this.m_FixedPane.style.flexBasis = (StyleLength) StyleKeyword.Null;
        this.m_FixedPane.style.flexShrink = (StyleFloat) StyleKeyword.Null;
        this.m_FixedPane.style.flexGrow = (StyleFloat) StyleKeyword.Null;
        this.m_FlexedPane.style.flexGrow = (StyleFloat) StyleKeyword.Null;
        this.m_FlexedPane.style.flexShrink = (StyleFloat) StyleKeyword.Null;
        this.m_FlexedPane.style.flexBasis = (StyleLength) StyleKeyword.Null;
        this.m_FixedPane.style.width = (StyleLength) StyleKeyword.Null;
        this.m_FixedPane.style.height = (StyleLength) StyleKeyword.Null;
        this.m_FlexedPane.style.width = (StyleLength) StyleKeyword.Null;
        this.m_FlexedPane.style.height = (StyleLength) StyleKeyword.Null;
        if (this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
        {
          this.m_FixedPane.style.width = (StyleLength) this.m_FixedPaneInitialDimension;
          this.m_FixedPane.style.height = (StyleLength) StyleKeyword.Null;
        }
        else
        {
          this.m_FixedPane.style.width = (StyleLength) StyleKeyword.Null;
          this.m_FixedPane.style.height = (StyleLength) this.m_FixedPaneInitialDimension;
        }
        this.m_FixedPane.style.flexShrink = (StyleFloat) 0.0f;
        this.m_FixedPane.style.flexGrow = (StyleFloat) 0.0f;
        this.m_FlexedPane.style.flexGrow = (StyleFloat) 1f;
        this.m_FlexedPane.style.flexShrink = (StyleFloat) 0.0f;
        this.m_FlexedPane.style.flexBasis = (StyleLength) 0.0f;
        if (this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
          this.m_DragLineAnchor.style.left = this.m_FixedPaneIndex != 0 ? (StyleLength) (this.resolvedStyle.width - this.m_FixedPaneInitialDimension) : (StyleLength) this.m_FixedPaneInitialDimension;
        else
          this.m_DragLineAnchor.style.top = this.m_FixedPaneIndex != 0 ? (StyleLength) (this.resolvedStyle.height - this.m_FixedPaneInitialDimension) : (StyleLength) this.m_FixedPaneInitialDimension;
        int dir = this.m_FixedPaneIndex != 0 ? -1 : 1;
        this.m_Resizer = this.m_FixedPaneIndex != 0 ? new CustomTwoPaneSplitViewResizer(this, dir, this.m_Orientation) : new CustomTwoPaneSplitViewResizer(this, dir, this.m_Orientation);
        this.m_DragLineAnchor.AddManipulator((IManipulator) this.m_Resizer);
        this.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnPostDisplaySetup));
        this.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnSizeChange));
      }
    }

    private void OnSizeChange(GeometryChangedEvent evt) => this.OnSizeChange();

    private void OnSizeChange()
    {
      if (this.m_CollapseMode)
        return;
      float num1 = this.resolvedStyle.width;
      float num2 = this.m_FixedPane.resolvedStyle.width;
      float dimension1 = this.m_FixedPane.resolvedStyle.minWidth.value;
      float num3 = this.m_FlexedPane.resolvedStyle.minWidth.value;
      if (this.m_Orientation == TwoPaneSplitViewOrientation.Vertical)
      {
        num1 = this.resolvedStyle.height;
        num2 = this.m_FixedPane.resolvedStyle.height;
        StyleFloat minHeight = this.m_FixedPane.resolvedStyle.minHeight;
        dimension1 = minHeight.value;
        minHeight = this.m_FlexedPane.resolvedStyle.minHeight;
        num3 = minHeight.value;
      }
      if ((double) num1 >= (double) num2 + (double) num3)
        this.SetDragLineOffset(this.m_FixedPaneIndex == 0 ? num2 : num1 - num2);
      else if ((double) num1 >= (double) dimension1 + (double) num3)
      {
        float dimension2 = num1 - num3;
        this.SetFixedPaneDimension(dimension2);
        this.SetDragLineOffset(this.m_FixedPaneIndex == 0 ? dimension2 : num3);
      }
      else
      {
        this.SetFixedPaneDimension(dimension1);
        this.SetDragLineOffset(this.m_FixedPaneIndex == 0 ? dimension1 : num3);
      }
    }

    public override VisualElement contentContainer => this.m_Content;

    private void SetDragLineOffset(float offset)
    {
      if (this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
        this.m_DragLineAnchor.style.left = (StyleLength) offset;
      else
        this.m_DragLineAnchor.style.top = (StyleLength) offset;
    }

    private void SetFixedPaneDimension(float dimension)
    {
      if (this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
        this.m_FixedPane.style.width = (StyleLength) dimension;
      else
        this.m_FixedPane.style.height = (StyleLength) dimension;
    }

    public new class UxmlFactory : UnityEngine.UIElements.UxmlFactory<CustomTwoPaneSplitView, CustomTwoPaneSplitView.UxmlTraits>
    {
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
      private UxmlIntAttributeDescription m_FixedPaneIndex;
      private UxmlIntAttributeDescription m_FixedPaneInitialDimension;
      private UxmlEnumAttributeDescription<TwoPaneSplitViewOrientation> m_Orientation;

      public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
      {
        get
        {
          yield break;
        }
      }

      public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
      {
        base.Init(ve, bag, cc);
        int valueFromBag1 = this.m_FixedPaneIndex.GetValueFromBag(bag, cc);
        int valueFromBag2 = this.m_FixedPaneInitialDimension.GetValueFromBag(bag, cc);
        TwoPaneSplitViewOrientation valueFromBag3 = this.m_Orientation.GetValueFromBag(bag, cc);
        ((CustomTwoPaneSplitView) ve).Init(valueFromBag1, (float) valueFromBag2, valueFromBag3);
      }

      public UxmlTraits()
      {
        UxmlIntAttributeDescription attributeDescription1 = new UxmlIntAttributeDescription();
        attributeDescription1.name = "fixed-pane-index";
        attributeDescription1.defaultValue = 0;
        this.m_FixedPaneIndex = attributeDescription1;
        UxmlIntAttributeDescription attributeDescription2 = new UxmlIntAttributeDescription();
        attributeDescription2.name = "fixed-pane-initial-dimension";
        attributeDescription2.defaultValue = 100;
        this.m_FixedPaneInitialDimension = attributeDescription2;
        UxmlEnumAttributeDescription<TwoPaneSplitViewOrientation> attributeDescription3 = new UxmlEnumAttributeDescription<TwoPaneSplitViewOrientation>();
        attributeDescription3.name = "orientation";
        attributeDescription3.defaultValue = TwoPaneSplitViewOrientation.Horizontal;
        this.m_Orientation = attributeDescription3;
        // // ISSUE: explicit constructor call
        // base.\u002Ector();
      }
    }
  }
}
