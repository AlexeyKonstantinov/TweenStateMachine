using TweensStateMachine.EditorScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TweensStateMachine
{
    public class TrackElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TrackElement, UxmlTraits> {}
        
        private SerializedProperty _animProperty;
        private Track _track;
        private bool _pointerDown;
        
        public VisualElement _leftHandle;
        public VisualElement _rightHandle;
        public float Snap = 0.05f;
        public float StartTime { get; private set; } = 1f;
        public float EndTime { get; private set; } = 2f;
        public float SecondsPerPixel => _track.SecondsPerPixel;

        public TrackElement()
        {
            AddToClassList("track-element");
        }
        
        public TrackElement(SerializedProperty animProperty, Track track)
        {
            AddToClassList("track-element");
            _animProperty = animProperty;
            _track = track;
            StartTime = _animProperty.FindPropertyRelative("delay").floatValue;
            EndTime = StartTime + _animProperty.FindPropertyRelative("duration").floatValue;

            _leftHandle = new VisualElement();
            _leftHandle.AddToClassList("track-element-left-handle");
            _rightHandle = new VisualElement();
            _rightHandle.AddToClassList("track-element-right-handle");
            Add(_leftHandle);
            Add(_rightHandle);

            this.AddManipulator(new TrackElementDragger());
            _leftHandle.AddManipulator(new LeftHandleDragger(this));
            _rightHandle.AddManipulator(new RightHandleDragger(this));
        }

        public void SetStartTime(float seconds)
        {
            seconds = Mathf.Clamp(seconds, 0, EndTime - Snap);
            StartTime = seconds;
            _animProperty.SetDelayProperty(seconds);
            _animProperty.SetDurationProperty(EndTime - StartTime);
            _animProperty.serializedObject.ApplyModifiedProperties();
            _animProperty.serializedObject.Update();
            UpdateView();
        }

        public void SetEndTime(float seconds)
        {
            seconds = Mathf.Clamp(seconds, StartTime + Snap, float.MaxValue);
            EndTime = seconds;
            _animProperty.SetDurationProperty(EndTime - StartTime);
            _animProperty.serializedObject.ApplyModifiedProperties();
            _animProperty.serializedObject.Update();
            UpdateView();
        }

        public void SetStartTimeKeepDuration(float seconds)
        {
            seconds = Mathf.Clamp(seconds, 0, float.MaxValue);
            StartTime = seconds;
            _animProperty.SetDelayProperty(seconds);
            EndTime = StartTime + _animProperty.GetDurationProperty();
            _animProperty.serializedObject.ApplyModifiedProperties();
            _animProperty.serializedObject.Update();
            UpdateView();
        }

        public void UpdateView()
        {
            style.left = (StartTime - _track.StartingPosition) / _track.SecondsPerPixel;
            style.width = (EndTime - StartTime) / _track.SecondsPerPixel;
        }
    }
}