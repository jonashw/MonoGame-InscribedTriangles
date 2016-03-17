using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiks.Entities
{
    public class GraphGrid
    {
        private readonly Vector3 _origin3;
        private readonly float _interval;
        private readonly int _size;
        private readonly Texture2D _pixelTexture;
        private readonly Texture2D _pointTexture;
        private readonly Texture2D _originTexture;
        private readonly Vector3[] _points;

        public GraphGrid(GraphicsDevice graphics, Vector2? maybeOrigin = null, float interval = 0.1f, int size = 10)
        {
            var origin = maybeOrigin.GetValueOrDefault(new Vector2(0, 0));
            _origin3 = new Vector3(origin.X, origin.Y, 0);
            _interval = interval;
            _size = size;

            _points = Enumerable.Range(-size, 2*size + 1).SelectMany(x =>
                Enumerable.Range(-size, 2*size + 1).Select(y =>
                    new Vector3(x*interval, y*interval, 0)))
                .Where(p => p != _origin3).ToArray();

            _pixelTexture = new Texture2D(graphics,1,1);
            _pixelTexture.SetData(new[] {Color.White});

            _pointTexture = new Texture2D(graphics,5,5);
            _pointTexture.SetData(new[]
            {
                Color.Transparent, Color.Transparent, Color.White, Color.Transparent,  Color.Transparent,
                Color.Transparent, Color.Transparent, Color.White, Color.Transparent,  Color.Transparent,
                Color.White,       Color.White,       Color.White, Color.White,        Color.White,
                Color.Transparent, Color.Transparent, Color.White, Color.Transparent,  Color.Transparent,
                Color.Transparent, Color.Transparent, Color.White, Color.Transparent,  Color.Transparent
            });

            _originTexture = new Texture2D(graphics,3,3);
            _originTexture.SetData(Enumerable.Range(0, _originTexture.Width * _originTexture.Height).Select(_ => Color.White).ToArray());
        }

        public void Draw( SpriteBatch spriteBatch, GraphicsDevice graphics, Matrix projectionMatrix, Matrix viewMatrix)
        {
            foreach(var point in _points)
            {
                drawSpriteAt(_pixelTexture, point, spriteBatch, graphics, projectionMatrix, viewMatrix);
            }
            drawSpriteAt(_originTexture, _origin3, spriteBatch, graphics, projectionMatrix, viewMatrix);
        }

        private static void drawSpriteAt(
            Texture2D texture,
            Vector3 position,
            SpriteBatch spriteBatch,
            GraphicsDevice graphics,
            Matrix projectionMatrix,
            Matrix viewMatrix)
        {
            var screenLocation = graphics.Viewport.Project(
                position,
                projectionMatrix,
                viewMatrix,
                Matrix.Identity);

            spriteBatch.Draw(
                texture,
                new Vector2(screenLocation.X, screenLocation.Y),
                origin: new Vector2(
                    texture.Width/2f,
                    texture.Height/2f));
        }
    }
}