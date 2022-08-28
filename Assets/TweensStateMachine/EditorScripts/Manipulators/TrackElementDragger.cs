using UnityEngine.UIElements;

namespace TweensStateMachine.EditorScripts
{
    public class TrackElementDragger : MouseManipulator
    {
        private bool _active;
        private float _startMouse;
        private float _startTime;
        
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
            if (!(target is TrackElement trackElement)) {
                return;
            }
            
            _active = true;
            _startMouse = evt.position.x;
            _startTime = trackElement.StartTime;
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void PointerUp(PointerUpEvent evt)
        {
            if (!(target is TrackElement)) {
                return;
            }
            
            _active = false;
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void PointerMove(PointerMoveEvent evt)
        {
            if (!(target is TrackElement trackElement)) {
                return;
            }
            
            if (!_active)
            {
                evt.StopImmediatePropagation();
                return;
            }

            var deltaPixels = evt.position.x - _startMouse;
            var deltaTime = deltaPixels * trackElement.SecondsPerPixel;
            var newStartTime = _startTime + deltaTime;
            newStartTime -= newStartTime % trackElement.Snap;
            trackElement.SetStartTimeKeepDuration(newStartTime);
            evt.StopPropagation();
        }
    }
}