using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameRubiks.Entities;

namespace MonoGameRubiks
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        private EquilateralTriangle _triangle;
        private GraphGrid _graphGrid;
        private SpriteFont _font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 720,
                PreferredBackBufferWidth = 1280
            };
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _basicEffect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                // Transform your model to place it somewhere in the world
                //World = Matrix.CreateRotationZ(MathHelper.PiOver4) * Matrix.CreateTranslation(0.5f, 0, 0), // for sake of example
                // Transform the entire world around (effectively: place the camera)
                View = Matrix.CreateLookAt(new Vector3(0, 0, -4), Vector3.Zero, Vector3.Up),
                // Specify how 3D points are projected/transformed onto the 2D screen
                Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 100.0f)
            };
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            //GraphicsDevice.BlendState = BlendState.Opaque;
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            _font = Content.Load<SpriteFont>("Consolas");
            _triangle = new EquilateralTriangle(1.5f, _font);
            _graphGrid = new GraphGrid(GraphicsDevice, size:14);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //vertices[0].Position.X += 0.1f;
            _triangle.Update(gameTime);

            base.Update(gameTime);
        }

        private readonly RasterizerState _state = new RasterizerState
        {
            FillMode = FillMode.WireFrame
        };

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _graphGrid.Draw(_spriteBatch, GraphicsDevice, _basicEffect.Projection, _basicEffect.View);

            GraphicsDevice.RasterizerState = _state;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            _triangle.Draw(_spriteBatch,GraphicsDevice);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}