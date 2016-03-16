using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiks.Entities
{
    public class EquilateralTriangle
    {
        private float _sideLength;
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

        public EquilateralTriangle(float sideLength)
        {
            _sideLength = sideLength;
            _vertices = getVertices(sideLength);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, 1, VertexPositionColor.VertexDeclaration);
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