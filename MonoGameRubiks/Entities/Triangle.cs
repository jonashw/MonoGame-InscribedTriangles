using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameInscribedTriangles.Entities
{
    public abstract class Triangle
    {
        private readonly Texture2D _pointTexture;
        protected VertexPositionColor[] Vertices;
        public bool DrawVertices = true;

        public Vector3 VertexA { get; protected set; }
        public Vector3 VertexB { get; protected set; }
        public Vector3 VertexC { get; protected set; }

        public float SlopeAB
        {
            get { return (VertexB.Y - VertexA.Y)/(VertexB.X - VertexA.X); }
        }

        public float SlopeBC
        {
            get { return (VertexC.Y - VertexB.Y)/(VertexC.X - VertexB.X); }
        }

        public float SlopeCA {
            get { return (VertexA.Y - VertexC.Y)/(VertexA.X - VertexC.X); }
        }

        protected Triangle(Texture2D pointTexture)
        {
            _pointTexture = pointTexture;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, Matrix projectionMatrix, Matrix viewMatrix)
        {
            graphics.DrawUserPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length/3, VertexPositionColor.VertexDeclaration);
            if (!DrawVertices)
            {
                return;
            }

            foreach (
                var screenLocation in 
                    Vertices.Select(vertex => graphics.Viewport.Project(
                        vertex.Position,
                        projectionMatrix,
                        viewMatrix,
                        Matrix.Identity)))
            {
                spriteBatch.Draw(_pointTexture, new Vector2(screenLocation.X, screenLocation.Y),
                    origin: new Vector2(_pointTexture.Width/2f, _pointTexture.Height/2f));
            }
        }
    }
}