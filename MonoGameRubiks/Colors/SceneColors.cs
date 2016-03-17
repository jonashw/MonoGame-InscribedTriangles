using Microsoft.Xna.Framework;

namespace MonoGameRubiks.Colors
{
    public static class SceneColors
    {
        public static Color Background { get { return Colors.Background; } }
        public static Color OuterTriangle { get { return Colors.OuterTriangle; } }
        public static Color ConnectorTriangles { get { return Colors.ConnectorTriangles; } }
        public static Color RotatingTriangle { get { return Colors.RotatingTriangle; } }
        public static Color InnerMostTriangle { get { return Colors.InnerMostTriangle; } }
        public static readonly Color Point = new Color(120, 120, 120);

        private static readonly SceneColorSet SetA = new SceneColorSet(
            Color.White,
            new Color(255, 192, 126),
            new Color(87, 149, 170),
            new Color(255, 217, 126),
            new Color(102, 116, 183));

        private static readonly SceneColorSet SetB = new SceneColorSet(
            new Color(200,100,100), 
            new Color(219, 188, 188),
            new Color(219, 209, 188),
            new Color(128, 134, 147),
            new Color(150, 175, 150));

        private static readonly SceneColorSet SetC = new SceneColorSet(
            new Color(60,60,60), 
            new Color(255, 0, 0),
            new Color(255,170,0),
            new Color(3,54,173),
            new Color(0,206,0));

        private static readonly MutableSceneColorSet Colors = MutableSceneColorSet.From(SetA);

        private static readonly CircularArray<SceneColorSetPair> TransitionPairs = new CircularArray<SceneColorSetPair>(
            new SceneColorSetPair(SetA, SetB),
            new SceneColorSetPair(SetB, SetC),
            new SceneColorSetPair(SetC, SetA));

        private readonly static SceneColorAnimator Animator =
            new SceneColorAnimator(
                Colors,
                TransitionPairs.GetCurrent(),
                1,
                SceneColorType.Background,
                SceneColorType.Outer,
                SceneColorType.Connector,
                SceneColorType.Rotating,
                SceneColorType.InnerMost);


        private readonly static EasyTimer WaitingTimer = new EasyTimer(3);
        private static bool _transitioning;
        public static void Update(GameTime gameTime)
        {
            if (_transitioning)
            {
                Animator.Update(gameTime);
                if (Animator.State != AnimatorState.Finished)
                {
                    return;
                }
                TransitionPairs.Next();
                Animator.ColorSetPair = TransitionPairs.GetCurrent();
                Animator.Reset();
                _transitioning = false;
            }
            else
            {
                WaitingTimer.Update(gameTime);
                if (!WaitingTimer.Finished)
                {
                    return;
                }
                WaitingTimer.Reset();
                _transitioning = true;
            }
        }
    }
}