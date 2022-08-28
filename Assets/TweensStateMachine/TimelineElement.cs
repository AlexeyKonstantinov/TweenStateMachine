using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TweensStateMachine
{
    public class TimelineElement : IMGUIContainer, ITimeline
    {
        private readonly List<float> _tickModulos = new List<float>
        {
            0.01f,
            0.05f,
            0.1f,
            0.5f,
            1f,
            5f,
            10f,
            50f,
            100f,
            500f,
            1000f,
            5000f,
            10000f,
            50000f
        };
        
        public new class UxmlFactory : UxmlFactory<TimelineElement, UxmlTraits> {}

        public float StartingPosition { get; set; }
        public float SecondsPerPixel { get; set; }

        public TimelineElement()
        {
            style.height = 40;
            onGUIHandler += ImmediateRepaint;
        }

        private void ImmediateRepaint()
        {
            Rect layout1 = layout;
            var mat = Resources.Load<Material>("Gizmos");

            var majorTick = MajorTicks(SecondsPerPixel);
            var startingMajorTick = StartingPosition - StartingPosition % majorTick + majorTick;
            var totalTime = StartingPosition + SecondsPerPixel * layout1.width;
            for (float tick = startingMajorTick; tick < totalTime; tick += majorTick)
            {
                var seconds = tick - StartingPosition;
                
                GUI.Label(new Rect(seconds / SecondsPerPixel, 20, 30, 15), $"{tick.ToString("n2")}",
                    new GUIStyle(EditorStyles.label) {fontSize = 7});
                    
                GL.Begin(GL.LINES);
                mat.SetPass(0);
                GL.Color(new Color(1f, 0.55f, 0.3f, 1f));
                GL.Vertex(new Vector3(seconds / SecondsPerPixel, layout1.yMin));
                GL.Vertex(new Vector3(seconds / SecondsPerPixel, 10000));
                GL.End();
            }

            if (SecondsPerPixel < 0.001) {
                return;
            }
            var halfTick = HalfTicks(SecondsPerPixel);
            var startingHalfTick = StartingPosition - StartingPosition % halfTick + halfTick;
            for (float tick = startingHalfTick; tick < totalTime; tick += halfTick)
            {
                var seconds = tick - StartingPosition;
                
                GUI.Label(new Rect(seconds / SecondsPerPixel, 20, 30, 15), $"{tick.ToString("n2")}",
                    new GUIStyle(EditorStyles.label) {fontSize = 7});
                    
                GL.Begin(GL.LINES);
                mat.SetPass(0);
                GL.Color(new Color(1f, 0.55f, 0.3f, 1f));
                GL.Vertex(new Vector3(seconds / SecondsPerPixel, layout1.yMin));
                GL.Vertex(new Vector3(seconds / SecondsPerPixel, layout1.yMax * 0.5f));
                GL.End();
            }

            if (SecondsPerPixel < 0.002) {
                return;
            }
            var decimalTick = DecimalTicks(SecondsPerPixel);
            var startingDecimalTick = StartingPosition - StartingPosition % decimalTick + decimalTick;
            for (float tick = startingDecimalTick; tick < totalTime; tick += decimalTick)
            {
                var seconds = tick - StartingPosition;
                    
                GL.Begin(GL.LINES);
                mat.SetPass(0);
                GL.Color(new Color(1f, 0.55f, 0.3f, 1f));
                GL.Vertex(new Vector3(seconds / SecondsPerPixel, layout1.yMin));
                GL.Vertex(new Vector3(seconds / SecondsPerPixel, layout1.yMax * 0.15f));
                GL.End();
            }
        }

        public void SetSecondsPerPixel(float seconds)
        {
            SecondsPerPixel = seconds;
        }

        public void SetStartingPosition(float ms)
        {
            StartingPosition = ms;
        }
        
        private float MajorTicks(float spp)
        {
            if(spp <= 0.0002)
                return _tickModulos[0];
            if (spp < 0.001)
                return _tickModulos[1];
            if (spp < 0.002)
                return _tickModulos[2];
            if (spp < 0.01)
                return _tickModulos[3];
            if (spp < 0.02f)
                return _tickModulos[4];
            if (spp < 0.1f)
                return _tickModulos[5];
            if (spp < 0.2f)
                return _tickModulos[6];
            if (spp < 1f)
                return _tickModulos[7];
            if (spp < 2f)
                return _tickModulos[8];
            if (spp < 10f)
                return _tickModulos[9];
            if (spp < 20f)
                return _tickModulos[10];
            if (spp < 100f)
                return _tickModulos[11];
            if (spp < 200f)
                return _tickModulos[12];
            return _tickModulos[13];
        }
        
        private float HalfTicks(float spp)
        {
            return MajorTicks(spp) * 0.5f;
        }

        private float DecimalTicks(float spp)
        {
            return MajorTicks(spp) * 0.1f;
        }
    }
}