using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameInscribedTriangles.Colors;
using MonoGameInscribedTriangles.Entities;

namespace MonoGameInscribedTriangles
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        private EquilateralTriangle _triangle;
        private InscribedEquilateralTriangle _inscribedTriangle;
        private Animator _triangleAnimator;
        private GraphGrid _graphGrid;
        private SpriteFont _font;
        private Texture2D _pointTexture;
        private readonly KeyboardEvents _keyboard = new KeyboardEvents();
        private readonly CircularArray<EasingFunction> _easingFn = new CircularArray<EasingFunction>(EasingFunction.All);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 720,
                PreferredBackBufferWidth = 1280
            };
            Content.RootDirectory = "Content";
        }

        private bool _updating = true;
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

            _pointTexture = GeometricTextureFactory.Circle(GraphicsDevice, 5, SceneColors.Point);
            _triangle = new EquilateralTriangle(_pointTexture, 2f);
            _inscribedTriangle = new InscribedEquilateralTriangle(
                _pointTexture,
                _triangle);
            _easingFn.Next(3);
            _triangleAnimator = new Animator(
                _easingFn.GetCurrent().Apply,
                _triangle.SideLength,
                value => _triangle.SideLength = value,
                .125f,
                0.5);

            _graphGrid = new GraphGrid(GraphicsDevice, size:30);

            _keyboard.OnPress(Keys.Right, () =>
            {
                _easingFn.Next();
                _triangleAnimator.Easing = _easingFn.GetCurrent().Apply;
            });

            _keyboard.OnPress(Keys.Left, () =>
            {
                _easingFn.Prev();
                _triangleAnimator.Easing = _easingFn.GetCurrent().Apply;
            });

            _keyboard.OnPress(Keys.W, toggleWireframeDisplay);

            _keyboard.OnPress(Keys.Up, () =>
            {
                _triangleAnimator.Duration += 0.5f;
            });

            _keyboard.OnPress(Keys.J, () => _inscribedTriangle.StepTheta(0.01f));
            _keyboard.OnPress(Keys.K, () => _inscribedTriangle.StepTheta(-.01f));

            _keyboard.OnPress(Keys.Down, () =>
            {
                _triangleAnimator.Duration -= 0.5f;
            });

            _keyboard.OnPress(Keys.Space, () => _updating = !_updating);

            toSolidDisplay();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _keyboard.Update(Keyboard.GetState());

            if (_updating)
            {
                //_triangleAnimator.Update(gameTime);

                _inscribedTriangle.Update();

                if (_triangleAnimator.State == AnimatorState.Finished)
                {
                    _triangleAnimator.StartingValue = _triangle.SideLength;
                    _triangleAnimator.ValueChange = -_triangleAnimator.ValueChange;
                    _triangleAnimator.Reset();
                }

                SceneColors.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private static readonly RasterizerState WireFrameRasterizerState = new RasterizerState
        {
            FillMode = FillMode.WireFrame,
            CullMode = CullMode.None,
            MultiSampleAntiAlias = true
        };

        private static readonly RasterizerState SolidFrameRasterizerState = new RasterizerState
        {
            FillMode = FillMode.Solid,
            CullMode = CullMode.None,
            MultiSampleAntiAlias = true
        };

        private void toggleWireframeDisplay()
        {
            if (_rasterizerState == WireFrameRasterizerState)
            {
                toSolidDisplay();
            }
            else
            {
                toWireFrameDisplay();
            }
        }

        private void toSolidDisplay()
        {
            _rasterizerState = SolidFrameRasterizerState;
            _inscribedTriangle.DrawVertices = false;
            _triangle.DrawVertices = false;
            _drawGrid = false;
        }

        private void toWireFrameDisplay()
        {
            _rasterizerState = WireFrameRasterizerState;
            _inscribedTriangle.DrawVertices = true;
            _triangle.DrawVertices = true;
            _drawGrid = true;
        }

        private RasterizerState _rasterizerState = SolidFrameRasterizerState;
        private bool _drawGrid = true;

        protected override void Draw(GameTime gameTime)
        {
            _graphics.PreferMultiSampling = true;
            GraphicsDevice.Clear(SceneColors.Background);

            _spriteBatch.Begin();
            if (_drawGrid)
            {
                _graphGrid.Draw(_spriteBatch, GraphicsDevice, _basicEffect.Projection, _basicEffect.View);
            }
            /*
            _spriteBatch.DrawString(_font, "Easing Function: " + _easingFn.GetCurrent().Name, new Vector2(24,12), Color.DarkBlue);
            _spriteBatch.DrawString(_font, "Animation Duration: " + _triangleAnimator.Duration, new Vector2(24,36), Color.DarkBlue);
            _spriteBatch.DrawString(_font, "Inscribed Triangle, Theta: " + _inscribedTriangle.Theta, new Vector2(24,60), Color.DarkBlue);
            foreach (var pair in new[]
            {
                _inscribedTriangle.VertexA,
                _inscribedTriangle.VertexB,
                _inscribedTriangle.VertexC
            }.Zip(Enumerable.Range(0,10), (v, i) => new {v,i}))
            {
                _spriteBatch.DrawString(_font,
                    string.Format("Inscribed Triangle, Vertex [{0},{1}]", pair.v.X, pair.v.Y),
                    new Vector2(24, 84 + 24*pair.i),
                    Color.DarkBlue);
            }
            */

            GraphicsDevice.RasterizerState = _rasterizerState;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            _triangle.Draw(_spriteBatch,GraphicsDevice, _basicEffect.Projection, _basicEffect.View);
            _inscribedTriangle.Draw(_spriteBatch, GraphicsDevice, _basicEffect.Projection, _basicEffect.View);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}