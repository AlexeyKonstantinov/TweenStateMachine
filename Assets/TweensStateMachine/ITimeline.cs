namespace TweensStateMachine
{
    public interface ITimeline
    {
        float StartingPosition { get; }
        float SecondsPerPixel { get; }
        void SetSecondsPerPixel(float seconds);
        void SetStartingPosition(float seconds);
    }
}