namespace MonoGameRubiks.Colors
{
    public class SceneColorSetPair
    {
        public readonly SceneColorSet Starting;
        public readonly SceneColorSet Ending;

        public SceneColorSetPair(SceneColorSet starting, SceneColorSet ending)
        {
            Starting = starting;
            Ending = ending;
        }
    }
}