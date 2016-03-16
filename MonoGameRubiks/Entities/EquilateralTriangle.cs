using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiks.Entities
{
    public class EquilateralTriangle
    {
        private float _sideLength;
        private readonly float _originalSideLength;
        private readonly SpriteFont _font;
        public float SideLength
        {
            get { return _sideLength; }
            set
            {
                _sideLength = value;
                updateVertices();
            }
        }
        private VertexPositionColor[] _vertices;

        public EquilateralTriangle(float sideLength, SpriteFont font)
        {
            _originalSideLength = sideLength;
            _font = font;
            _sideLength = sideLength;
            _vertices = getVertices(sideLength);
            _animationOriginalSideLength = sideLength;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.DrawString(_font, "Easing Function: " + _easingFn.GetCurrent().Name, new Vector2(24,24), Color.White);
            graphics.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, 1, VertexPositionColor.VertexDeclaration);
        }

        private const double MovementDuration = 2f;
        private double _currentAnimationTime = 0f;
        private bool _growing = true;
        private double _animationOriginalSideLength;
        private readonly CircularArray<EasingFunction> _easingFn = new CircularArray<EasingFunction>(EasingFunction.All);
        public void Update(GameTime gameTime)
        {
            var b = _animationOriginalSideLength;
            var c = (_growing ? 1 : -1) * _originalSideLength/2;
            var d = MovementDuration;
            var t = _currentAnimationTime = Math.Min(
                _currentAnimationTime + gameTime.ElapsedGameTime.TotalSeconds,
                d);

            SideLength = (float) _easingFn.GetCurrent().Apply(t, b, c, d);

            if (!(t >= d))
            {
                return;
            }
            if (!_growing)
            {
                _easingFn.Next();
            }
            _animationOriginalSideLength = SideLength;
            _currentAnimationTime = 0;
            _growing = !_growing;
        }

        private void updateVertices()
        {
            _vertices = getVertices(_sideLength);
        }

        private static VertexPositionColor[] getVertices(float sideLength)
        {
            var h = sideLength * (float)Math.Sqrt(3) / 2f;
            return new[]
            {
                new VertexPositionColor(new Vector3(sideLength/-2, -h/3, 0), Color.White),
                new VertexPositionColor(new Vector3(sideLength/2, -h/3, 0), Color.White),
                new VertexPositionColor(new Vector3(0.0f, (2f/3)*h, 0), Color.White)
            };
        }
    }

}