using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameInscribedTriangles.Colors
{
    public class SceneColorAnimator
    {
        public readonly MutableSceneColorSet Colors;

        public double Duration
        {
            get { return _timer.Duration; }
        }

        public SceneColorSetPair ColorSetPair; 
        private readonly IEnumerable<SceneColorType> _colorTypes;
        public AnimatorState State { get; private set; }
        private readonly EasyTimer _timer;

        public SceneColorAnimator(MutableSceneColorSet colors, SceneColorSetPair colorSetPair, float duration, params SceneColorType[] colorTypes)
        {
            Colors = colors;
            _colorTypes = colorTypes;
            ColorSetPair = colorSetPair;
            State = AnimatorState.Animating;
            _timer = new EasyTimer(duration);
        }

        public void Update(GameTime gameTime)
        {
            if (State != AnimatorState.Animating)
            {
                return;
            }

            _timer.Update(gameTime);

            foreach (var colorType in _colorTypes)
            {
                var startingColor = ColorSetPair.Starting.Get(colorType);
                var endingColor = ColorSetPair.Ending.Get(colorType);
                var nextColor = Color.Lerp(
                    startingColor,
                    endingColor,
                    _timer.Progress);

                Colors.Set(colorType, nextColor);
            }

            if (_timer.Finished)
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
            _timer.Reset();
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
}
