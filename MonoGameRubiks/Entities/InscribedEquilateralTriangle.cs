using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiks.Entities
{
    public class InscribedEquilateralTriangle : Triangle
    {
        public readonly Triangle ParentTriangle;
        private float _theta;

        public InscribedEquilateralTriangle(Texture2D pointTexture, Triangle parentTriangle) : base(pointTexture)
        {
            ParentTriangle = parentTriangle;
            _theta = 0;
            updateVertices();
        }

        public void Update()
        {
            updateVertices();
            _theta -= 0.01f;
            if (_theta > MathHelper.TwoPi)
            {
                _theta -= MathHelper.TwoPi;
            }
            if (_theta < 0)
            {
                _theta += MathHelper.TwoPi;
            }
        }

        private void updateVertices()
        {
            var v = getVertexPosition(0);
            Vertices =  new[]
            {
                VertexA = getVertexPosition(0),
                VertexB = getVertexPosition(2 * MathHelper.Pi/3),
                VertexC = getVertexPosition(4 * MathHelper.Pi/3)
            }.Select(p => new VertexPositionColor(p, Color.White)).ToArray();
        }

        private const float Top = MathHelper.Pi/2;
        private const float BottomLeft = MathHelper.Pi + MathHelper.Pi/6;
        private const float BottomRight = MathHelper.TwoPi - MathHelper.Pi/6;
        private const float BottomCenter = 3 * MathHelper.Pi/2;

        private Vector3 getVertexPosition(float thetaOffset)
        {
            var theta = (_theta + thetaOffset) % MathHelper.TwoPi;

            if (Math.Abs(theta - Top) < 0.01)
            {
                return ParentTriangle.VertexA;
            }
            if (Math.Abs(theta - BottomCenter) < 0.01)
            {
                return new Vector3(0, ParentTriangle.VertexC.Y, 0);
            }
            float slope, intercept;
            if (Top < theta && theta <= BottomLeft)
            {
                slope = ParentTriangle.SlopeAB;
                intercept = ParentTriangle.VertexA.Y;
            }
            else if (BottomLeft < theta && theta <= BottomRight)
            {
                slope = ParentTriangle.SlopeBC;
                intercept = ParentTriangle.VertexB.Y;
            }
            else
            {
                slope = ParentTriangle.SlopeCA;
                intercept = ParentTriangle.VertexA.Y;
            }

            var tan = (float)Math.Tan(theta);
            var x = intercept/(tan - slope);
            var y = x*tan;
            return new Vector3(x, y, 0);
        }
    }
}