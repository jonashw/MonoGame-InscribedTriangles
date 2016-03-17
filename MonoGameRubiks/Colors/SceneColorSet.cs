using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameRubiks.Colors
{
    public class SceneColorSet
    {
        protected readonly Dictionary<SceneColorType, Color> Colors;
        public Color Background { get { return Colors[SceneColorType.Background]; } }
        public Color OuterTriangle { get { return Colors[SceneColorType.Outer]; } }
        public Color ConnectorTriangles { get { return Colors[SceneColorType.Connector]; } }
        public Color RotatingTriangle { get { return Colors[SceneColorType.Rotating]; } }
        public Color InnerMostTriangle { get { return Colors[SceneColorType.InnerMost]; } }

        public Color Get(SceneColorType type)
        {
            return Colors[type];
        }

        protected SceneColorSet(SceneColorSet otherSet)
        {
            Colors = new Dictionary<SceneColorType, Color>(otherSet.Colors);
        }

        public SceneColorSet(Color background, Color outerTriangle, Color connectorTriangles, Color rotatingTriangle, Color innerMostTriangle)
        {
            Colors = new Dictionary<SceneColorType, Color>
                {
                    {SceneColorType.Background, background},
                    {SceneColorType.Outer, outerTriangle},
                    {SceneColorType.Connector, connectorTriangles},
                    {SceneColorType.Rotating, rotatingTriangle},
                    {SceneColorType.InnerMost, innerMostTriangle}
                };
        }
    }

    public class MutableSceneColorSet : SceneColorSet
    {
        public MutableSceneColorSet(
            Color background,
            Color outerTriangle,
            Color connectorTriangles,
            Color rotatingTriangle,
            Color innerMostTriangle) : base(
                background,
                outerTriangle,
                connectorTriangles,
                rotatingTriangle,
                innerMostTriangle){}

        protected MutableSceneColorSet(SceneColorSet set) : base(set){}

        public void Set(SceneColorType type, Color color)
        {
            Colors[type] = color;
        }

        public static MutableSceneColorSet From(SceneColorSet set)
        {
            return new MutableSceneColorSet(set);
        }
    }
}