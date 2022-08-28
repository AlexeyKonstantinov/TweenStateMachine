using UnityEngine.UIElements;

namespace TweensStateMachine.EditorScripts
{
    public class LeftHandleDragger : MouseManipulator
    {
        private TrackElement _trackElement;
        private bool _active;
        private float _startMouse;
        private float _startTime;

        public LeftHandleDragger(TrackElement trackElement)
        {
            _trackElement = trackElement;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(PointerDown);
            target.RegisterCallback<PointerUpEvent>(PointerUp);
            target.RegisterCallback<PointerMoveEvent>(PointerMove);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(PointerDown);
            target.UnregisterCallback<PointerUpEvent>(PointerUp);
            target.UnregisterCallback<PointerMoveEvent>(PointerMove);
        }

        private void PointerDown(PointerDownEvent evt)
        {
            _active = true;
            _startMouse = evt.position.x;
            _startTime = _trackElement.StartTime;
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void PointerUp(PointerUpEvent evt)
        {
            _active = false;
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void PointerMove(PointerMoveEvent evt)
        {
            if (!_active)
            {
                evt.StopImmediatePropagation();
                return;
            }

            var deltaPixels = evt.position.x - _startMouse;
            var deltaTime = deltaPixels * _trackElement.SecondsPerPixel;
            var newStartTime = _startTime + deltaTime;
            newStartTime -= newStartTime % _trackElement.Snap;
            _trackElement.SetStartTime(newStartTime);
            evt.StopPropagation();
        }
    }
}