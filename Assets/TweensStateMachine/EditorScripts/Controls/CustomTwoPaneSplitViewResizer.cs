namespace UnityEngine.UIElements
{
  public class CustomTwoPaneSplitViewResizer : MouseManipulator
  {
    private Vector2 m_Start;
    protected bool m_Active;
    private CustomTwoPaneSplitView m_SplitView;
    private int m_Direction;
    private TwoPaneSplitViewOrientation m_Orientation;

    private VisualElement fixedPane => this.m_SplitView.fixedPane;

    private VisualElement flexedPane => this.m_SplitView.flexedPane;

    private float fixedPaneMinDimension => this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal ? this.fixedPane.resolvedStyle.minWidth.value : this.fixedPane.resolvedStyle.minHeight.value;

    private float flexedPaneMinDimension => this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal ? this.flexedPane.resolvedStyle.minWidth.value : this.flexedPane.resolvedStyle.minHeight.value;

    public CustomTwoPaneSplitViewResizer(
      CustomTwoPaneSplitView splitView,
      int dir,
      TwoPaneSplitViewOrientation orientation)
    {
      this.m_Orientation = orientation;
      this.m_SplitView = splitView;
      this.m_Direction = dir;
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.LeftMouse
      });
      this.m_Active = false;
    }

    protected override void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown));
      this.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove));
      this.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp));
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown));
      this.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove));
      this.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp));
    }

    public void ApplyDelta(float delta)
    {
      float num1 = this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal ? this.fixedPane.resolvedStyle.width : this.fixedPane.resolvedStyle.height;
      float num2 = num1 + delta;
      if ((double) num2 < (double) num1 && (double) num2 < (double) this.fixedPaneMinDimension)
        num2 = this.fixedPaneMinDimension;
      float num3 = (this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal ? this.m_SplitView.resolvedStyle.width : this.m_SplitView.resolvedStyle.height) - this.flexedPaneMinDimension;
      if ((double) num2 > (double) num1 && (double) num2 > (double) num3)
        num2 = num3;
      if (this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
      {
        this.fixedPane.style.width = (StyleLength) num2;
        if (this.m_SplitView.fixedPaneIndex == 0)
          this.target.style.left = (StyleLength) num2;
        else
          this.target.style.left = (StyleLength) (this.m_SplitView.resolvedStyle.width - num2);
      }
      else
      {
        this.fixedPane.style.height = (StyleLength) num2;
        if (this.m_SplitView.fixedPaneIndex == 0)
          this.target.style.top = (StyleLength) num2;
        else
          this.target.style.top = (StyleLength) (this.m_SplitView.resolvedStyle.height - num2);
      }
    }

    protected void OnMouseDown(MouseDownEvent e)
    {
      if (this.m_Active)
      {
        e.StopImmediatePropagation();
      }
      else
      {
        if (!this.CanStartManipulation((IMouseEvent) e))
          return;
        this.m_Start = e.localMousePosition;
        this.m_Active = true;
        this.target.CaptureMouse();
        e.StopPropagation();
      }
    }

    protected void OnMouseMove(MouseMoveEvent e)
    {
      if (!this.m_Active || !this.target.HasMouseCapture())
        return;
      Vector2 vector2 = e.localMousePosition - this.m_Start;
      float num = vector2.x;
      if (this.m_Orientation == TwoPaneSplitViewOrientation.Vertical)
        num = vector2.y;
      this.ApplyDelta((float) this.m_Direction * num);
      e.StopPropagation();
    }

    protected void OnMouseUp(MouseUpEvent e)
    {
      if (!this.m_Active || !this.target.HasMouseCapture() || !this.CanStopManipulation((IMouseEvent) e))
        return;
      this.m_Active = false;
      this.target.ReleaseMouse();
      e.StopPropagation();
    }
  }
}