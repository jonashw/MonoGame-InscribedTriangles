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
        public AnimatorState State { get; private set; }

        public Animator(Easing.EasingFn easing, float startingValue, Action<float> setter, float valueChange, double duration)
        {
            Easing = easing;
            ValueChange = valueChange;
            StartingValue = startingValue;
            Duration = duration;
            _setter = setter;
            State = AnimatorState.Animating;
        }

        public void Update(GameTime gameTime)
        {
            if (State != AnimatorState.Animating)
            {
                return;
            }
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
                State = AnimatorState.Finished;
            }
        }

        public void Reset()
        {
            if (State != AnimatorState.Finished)
            {
                return;
            }
            _currentAnimationTime = 0;
            State = AnimatorState.Animating;
        }

        public void PlayPause()
        {
            switch (State)
            {
                case AnimatorState.Animating:
                    State = AnimatorState.Paused;
                    return;
                case AnimatorState.Paused:
                    State = AnimatorState.Animating;
                    return;
            }
        }
    }

    public enum AnimatorState
    {
        Animating, Paused, Finished
    }
}