using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameRubiks.Entities;

namespace MonoGameRubiks
{
    public class EquilateralTriangleAnimator
    {
        private readonly EquilateralTriangle _triangle;
        private readonly SpriteFont _font;
        private readonly float _originalSideLength;
        private readonly CircularArray<EasingFunction> _easingFn = new CircularArray<EasingFunction>(EasingFunction.All);

        private const double MovementDuration = 2f;
        private double _currentAnimationTime;
        private bool _growing = true;
        private double _animationOriginalSideLength;

        public EquilateralTriangleAnimator(EquilateralTriangle triangle, SpriteFont font)
        {
            _triangle = triangle;
            _font = font;
            _originalSideLength = triangle.SideLength;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Easing Function: " + _easingFn.GetCurrent().Name, new Vector2(24,12), Color.DarkBlue);
        }

        public void Update(GameTime gameTime)
        {
            var b = _animationOriginalSideLength;
            var c = (_growing ? 1 : -1) * _originalSideLength;
            var d = MovementDuration;
            var t = _currentAnimationTime = Math.Min(
                _currentAnimationTime + gameTime.ElapsedGameTime.TotalSeconds,
                d);

            _triangle.SideLength = (float) _easingFn.GetCurrent().Apply(t, b, c, d);

            if (!(t >= d))
            {
                return;
            }
            if (!_growing)
            {
                _easingFn.Next();
            }
            _animationOriginalSideLength = _triangle.SideLength;
            _currentAnimationTime = 0;
            _growing = !_growing;
        }
    }
}