using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameRubiks.Colors;

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

        public float Theta
        {
            get { return _theta; }
        }

        public void Update()
        {
            _theta += 0.01f;
            updateVertices();
            if (_theta > MathHelper.TwoPi)
            {
                _theta -= MathHelper.TwoPi;
            }
            if (_theta < 0)
            {
                _theta += MathHelper.TwoPi;
            }
        }

        public void StepTheta(float dTheta)
        {
            _theta -= dTheta;
            updateVertices();
        }

        private void updateVertices()
        {
            setVertexPosition(0);
            setVertexPosition(2*MathHelper.Pi/3);
            setVertexPosition(4*MathHelper.Pi/3);

            Vertices = new[]
            {
                triangle(
                    SceneColors.RotatingTriangle,
                    VertexA,
                    VertexB,
                    VertexC),

                triangle(
                    SceneColors.InnerMostTriangle,
                    (VertexB + VertexA)/2,
                    (VertexB + VertexC)/2,
                    (VertexA + VertexC)/2),

                triangle(
                    SceneColors.ConnectorTriangles,
                    ParentTriangle.VertexA,
                    (VertexC + VertexA)/2,
                    VertexC),

                triangle(
                    SceneColors.ConnectorTriangles,
                    ParentTriangle.VertexB,
                    (VertexB + VertexA)/2,
                    VertexA),

                triangle(
                    SceneColors.ConnectorTriangles,
                    ParentTriangle.VertexC,
                    (VertexB + VertexC)/2,
                    VertexB)
            }.SelectMany(t => t).ToArray();
        }

        private IEnumerable<VertexPositionColor> triangle(Color color, params Vector3[] vs)
        {
            return vs.Select(v => new VertexPositionColor(v, color));
        }

        private const float Top = MathHelper.Pi/2;
        private const float BottomLeft = MathHelper.Pi + MathHelper.Pi/6;
        private const float BottomRight = MathHelper.TwoPi - MathHelper.Pi/6;
        private const float BottomCenter = 3 * MathHelper.Pi/2;

        private const float Tolerance = 0.001f;
        private void setVertexPosition(float thetaOffset)
        {
            var theta = (_theta + thetaOffset) % MathHelper.TwoPi;

            if (Math.Abs(theta - Top) < Tolerance)
            {
                VertexA = ParentTriangle.VertexA;
            }
            else if (Math.Abs(theta - BottomCenter) < Tolerance)
            {
                VertexB = new Vector3(0, ParentTriangle.VertexC.Y, 0);
            } 
            else if (Top < theta && theta < BottomLeft)
            {
                VertexA = getVertex(theta, ParentTriangle.SlopeAB, ParentTriangle.VertexA.Y);
            }
            else if (BottomLeft < theta && theta < BottomRight)
            {
                VertexB = getVertex(theta, ParentTriangle.SlopeBC, ParentTriangle.VertexB.Y);
            }
            else
            {
                VertexC = getVertex(theta, ParentTriangle.SlopeCA, ParentTriangle.VertexA.Y);
            }
        }

        private static Vector3 getVertex(float theta, float slope, float intercept)
        {
            var tan = (float)Math.Tan(theta);
            var x = intercept/(tan - slope);
            var y = x*tan;
            return new Vector3(x, y, 0);
        }
    }
}