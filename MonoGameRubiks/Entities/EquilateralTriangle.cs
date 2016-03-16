using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiks.Entities
{
    public class EquilateralTriangle
    {
        private float _sideLength;
        private float _height;
        private VertexPositionColor[] _vertices;

        public EquilateralTriangle(float sideLength)
        {
            _sideLength = sideLength;
            updateHeight();
            updateVertices();
        }

        public float SideLength
        {
            get { return _sideLength; }
            set
            {
                _sideLength = value;
                updateHeight();
                updateVertices();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length/3, VertexPositionColor.VertexDeclaration);
        }

        private void updateHeight()
        {
            _height = _sideLength * (float)Math.Sqrt(3) / 2f;
        }

        private void updateVertices()
        {
            var h = _height;
            _vertices =  new[]
            {
                new Vector3(0.0f, (2f/3)*h, 0),
                new Vector3(-_sideLength/2, -h/3, 0),
                new Vector3( _sideLength/2, -h/3, 0),
            }.Select(p => new VertexPositionColor(p, Color.White)).ToArray();
        }
    }
}