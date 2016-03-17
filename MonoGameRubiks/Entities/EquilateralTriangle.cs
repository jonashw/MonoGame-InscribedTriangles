using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiks.Entities
{
    public class EquilateralTriangle : Triangle
    {
        private float _sideLength;
        public float Height { get; private set; }
        public float CircumRadius { get; private set; }

        public EquilateralTriangle(Texture2D pointTexture, float sideLength) : base(pointTexture)
        {
            _sideLength = sideLength;
            updateCircumRadius();
            updateHeight();
            updateVertices();
        }


        public float SideLength
        {
            get { return _sideLength; }
            set
            {
                _sideLength = value;
                updateCircumRadius();
                updateHeight();
                updateVertices();
            }
        }

        private void updateHeight()
        {
            Height = _sideLength * (float)Math.Sqrt(3) / 2f;
        }

        private void updateCircumRadius()
        {
            CircumRadius = _sideLength/(float) Math.Sqrt(3);
        }

        private void updateVertices()
        {
            var h = Height;
            var a = VertexA = new Vector3(0.0f, (2f/3)*h, 0);
            var b = VertexB = new Vector3(-_sideLength/2, -h/3, 0);
            var c = VertexC = new Vector3(_sideLength/2, -h/3, 0);
            Vertices = new[] {a, b, c}
                .Select(p => new VertexPositionColor(p, Color.White))
                .ToArray();
        }
    }
}