using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiks.Entities
{
    public class InscribedEquilateralTriangle : Triangle
    {
        public readonly EquilateralTriangle ParentTriangle;
        private float _theta;

        public InscribedEquilateralTriangle(Texture2D pointTexture, EquilateralTriangle parentTriangle) : base(pointTexture)
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

            if (0 < theta && theta < Top)
            {
                var tan = (float)Math.Tan(theta);
                var x = ParentTriangle.VertexA.Y/(tan - ParentTriangle.SlopeCA);
                var y = x*tan;
                return new Vector3(x, y, 0);
            }
            if (Math.Abs(theta - Top) < 0.01)
            {
                return ParentTriangle.VertexA;
            }
            if (Top < theta && theta < BottomLeft)
            {
                var alpha = theta - MathHelper.PiOver2;
                var tan = (float)Math.Tan(alpha);
                var x = -ParentTriangle.VertexA.Y/((1/tan) + ParentTriangle.SlopeAB);
                var y = -x/tan;
                return new Vector3(x, y, 0);
            }
            if (BottomLeft < theta && theta < BottomCenter)
            {
                theta = BottomCenter - theta;
                var tan = (float)Math.Tan(theta);
                var y = ParentTriangle.VertexC.Y;
                var x = y*tan;
                return new Vector3(x, y, 0);
            }
            if (BottomCenter < theta && theta < BottomRight)
            {
                var phi = theta - MathHelper.Pi;
                var y = ParentTriangle.VertexB.Y;
                var x = ParentTriangle.VertexB.Y/(float) Math.Tan(phi);
                return new Vector3(x, y, 0);
            }
            if (BottomRight < theta && theta < MathHelper.TwoPi)
            {
                var phi = MathHelper.TwoPi - theta;
                var tan = (float)Math.Tan(phi);
                var x = -ParentTriangle.VertexA.Y / (tan + ParentTriangle.SlopeCA);
                var y = -x*tan;
                return new Vector3(x, y, 0);
            }
            return new Vector3();
        }
    }
}