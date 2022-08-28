using UnityEditor;
using UnityEngine.UIElements;

namespace TweensStateMachine
{
    public class Track : VisualElement, ITimeline
    {
        public new class UxmlFactory : UxmlFactory<Track, UxmlTraits> {}

        private TrackElement _trackElement;

        public Track()
        {
            
        }
        
        public Track(SerializedProperty animProperty)
        {
            AddToClassList("track");
            style.height = 80;
            _trackElement = new TrackElement(animProperty, this);
            Add(_trackElement);
        }

        public float StartingPosition { get; private set; }
        public float SecondsPerPixel { get; private set; }
        
        public void SetSecondsPerPixel(float seconds)
        {
            SecondsPerPixel = seconds;
            _trackElement.UpdateView();
        }

        public void SetStartingPosition(float seconds)
        {
            StartingPosition = seconds;
            _trackElement.UpdateView();
        }
    }
}