using System;
using Microsoft.Xna.Framework;

namespace MonoGameRubiks
{
    public class Animator
    {
        public Easing.EasingFn Easing;
        public double Duration;
        public double StartingValue; 
        public float ValueChange;
        private readonly Action<float> _setter;
        private double _currentAnimationTime;

        public bool Animating { get; private set; }

        public Animator(Easing.EasingFn easing, float startingValue, Action<float> setter, float valueChange, double duration)
        {
            Easing = easing;
            ValueChange = valueChange;
            StartingValue = startingValue;
            Duration = duration;
            _setter = setter;
            Animating = true;
        }

        public void Update(GameTime gameTime)
        {
            _currentAnimationTime = Math.Min(
                _currentAnimationTime + gameTime.ElapsedGameTime.TotalSeconds,
                Duration); // Prevent over-animating

            var nextValue = (float) Easing(
                _currentAnimationTime,
                StartingValue,
                ValueChange,
                Duration);

            _setter(nextValue);

            if (_currentAnimationTime >= Duration)
            {
                Animating = false;
            }
        }

        public void Reset()
        {
            if (Animating)
            {
                return;
            }
            _currentAnimationTime = 0;
            Animating = true;
        }
    }
}