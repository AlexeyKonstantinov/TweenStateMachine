using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace TweensStateMachine
{
    public class CustomGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<CustomGraphView, UxmlTraits>{}

        public CustomGraphView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new UnityEditor.Experimental.GraphView.ContentDragger());
            this.AddManipulator(new RectangleSelector());
        }
    }
}