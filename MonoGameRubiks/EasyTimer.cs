using System;
using Microsoft.Xna.Framework;

namespace MonoGameRubiks
{
    public class EasyTimer
    {
        public float Duration;
        private double _secondsElapsed;

        public float Progress
        {
            get
            {
                return  Math.Min((float) (Math.Min(_secondsElapsed, Duration) / Duration), 1f);
            }
        }
        public bool Finished { get; private set; }
        public EasyTimer(float duration)
        {
            Duration = duration;
        }

        public void Update(GameTime gameTime)
        {
            if (Finished)
            {
                return;
            }
            _secondsElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (_secondsElapsed >= Duration)
            {
                Finished = true;
            }
        }

        public void Reset()
        {
            if (!Finished)
            {
                return;
            }
            _secondsElapsed = 0;
            Finished = false;
        }
    }
}