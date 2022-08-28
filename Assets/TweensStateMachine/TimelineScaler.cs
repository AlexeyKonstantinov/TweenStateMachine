using System;
using UnityEngine.UIElements;

namespace TweensStateMachine
{
    public class TimelineScaler : Manipulator
    {
        protected override void RegisterCallbacksOnTarget()
        {
            if (!(target is SequenceElement))
                throw new InvalidOperationException("Manipulator can only be added to a sequence");
            target.RegisterCallback<WheelEvent>(OnWheel);
        }

        protected override void UnregisterCallbacksFromTarget() =>
            target.UnregisterCallback<WheelEvent>(OnWheel);
        
        private void OnWheel(WheelEvent evt)
        {
            if (!(this.target is SequenceElement sequenceElement) 
                || (evt.target is VisualElement tgt ? tgt.panel : (IPanel) null).GetCapturingElement(PointerId.mousePointerId) != null)
                return;

            float sppDelta = 0.00001f;
            float newSpp = evt.delta.y > 0
                ? sequenceElement.SecondsPerPixel + sequenceElement.SecondsPerPixel/0.0004f * sppDelta
                : sequenceElement.SecondsPerPixel - sequenceElement.SecondsPerPixel/0.0004f * sppDelta;
            
            var x = evt.localMousePosition.x;
            var t = sequenceElement.StartingPosition + sequenceElement.SecondsPerPixel * x;
            sequenceElement.SetSecondsPerPixel(newSpp);
            var t0New = t - sequenceElement.SecondsPerPixel * x;

            sequenceElement.SetStartingPosition(t0New);
            evt.StopPropagation();
        }
    }
}