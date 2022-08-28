using UnityEngine.UIElements;

namespace TweensStateMachine.EditorScripts
{
    public class SplitView : MyTwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits>{}

        public SplitView()
        {
        }

        public SplitView(int fixedPaneIndex, float fixedPaneStartDimension, TwoPaneSplitViewOrientation orientation) : base(fixedPaneIndex, fixedPaneStartDimension, orientation)
        {
        }
    }
}