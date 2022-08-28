using UnityEngine.UIElements;

namespace TweensStateMachine
{
    public interface IDraggable
    {
        public void Drag(PointerMoveEvent evt);
    }
}