using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TweensStateMachine
{
    public class SequenceElement : VisualElement, ITimeline
    {
        public new class UxmlFactory : UxmlFactory<SequenceElement, UxmlTraits> {}

        public readonly float MinSecondsPerPixel = 0.0001f;
        public const float MinStartingPosition = 0;

        public float StartingPosition { get; set; } = 0;
        public float SecondsPerPixel { get; private set; } = 0.001f;

        public VisualElement TrackContainer;
        public TimelineElement TimelineElement;
        public Label ScaleLabel;

        private List<ITimeline> _timeScaleDependents;
        private List<Track> _tracks;

        public SequenceElement()
        {
            _timeScaleDependents = new List<ITimeline>();
            _tracks = new List<Track>();
            ScaleLabel = new Label();
            ScaleLabel.text = $"upp: {SecondsPerPixel.ToString()}";
            ScaleLabel.style.position = Position.Absolute;
            ScaleLabel.style.top = -20;
            ScaleLabel.style.visibility = Visibility.Hidden;
            
            TimelineElement = new TimelineElement();
            Insert(0, TimelineElement);
            TrackContainer = new VisualElement {name = "TracksContainer"};
            TrackContainer.Add(ScaleLabel);
            TrackContainer.style.flexGrow = 1;
            hierarchy.Add(TrackContainer);
            
            this.AddManipulator(new TimelineScaler());
            
            _timeScaleDependents.Add(TimelineElement);
        }

        public void ClearElement()
        {
            _timeScaleDependents.Clear();
            _timeScaleDependents.Add(TimelineElement);
            _tracks.Clear();
            TrackContainer.Clear();
        }

        public void AddTrack(SerializedProperty animProperty)
        {
            var track = new Track(animProperty);
            _tracks.Add(track);
            _timeScaleDependents.Add(track);
            TrackContainer.Add(track);
        }

        public void UpdateViewWithSerializedData()
        {
            foreach (var track in _tracks)
            {
                track.UpdateViewWithSerializedData();
            }
        }

        public void SetSecondsPerPixel(float seconds)
        {
            SecondsPerPixel = Mathf.Clamp(seconds, MinSecondsPerPixel, float.MaxValue);
            ScaleLabel.text = $"spp: {SecondsPerPixel.ToString()}, startingTimeMs: {StartingPosition}";
            foreach (var timeScaleDependent in _timeScaleDependents) {
                timeScaleDependent.SetSecondsPerPixel(SecondsPerPixel);
            }
        }

        public void SetStartingPosition(float seconds)
        {
            StartingPosition = Mathf.Clamp(seconds, MinStartingPosition, float.MaxValue);
            foreach (var timeScaleDependent in _timeScaleDependents) {
                timeScaleDependent.SetStartingPosition(StartingPosition);
            }
        }
    }
}