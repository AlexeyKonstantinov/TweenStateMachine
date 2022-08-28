using System;
using UnityEngine.UIElements;

namespace TweensStateMachine
{
    public class ContentDragger : Manipulator
    {
        private bool _pointerDown;
        
        protected override void RegisterCallbacksOnTarget()
        {
            if (!(target is IDraggable))
                throw new InvalidOperationException("Manipulator 'ContentDragger' can only be added to IDraggable");
            target.RegisterCallback<PointerDownEvent>(PointerDown);
            target.RegisterCallback<PointerMoveEvent>(PointerMove);
            target.RegisterCallback<PointerUpEvent>(PointerUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(PointerDown);
            target.UnregisterCallback<PointerMoveEvent>(PointerMove);
            target.UnregisterCallback<PointerUpEvent>(PointerUp);
        }

        private void PointerDown(PointerDownEvent evt)
        {
            _pointerDown = true;
        }

        private void PointerMove(PointerMoveEvent evt)
        {
            if (!_pointerDown) {
                return;
            }

            var draggable = (IDraggable) target;
            draggable.Drag(evt);
        }

        private void PointerUp(PointerUpEvent evt)
        {
            _pointerDown = false;
        }
    }
}