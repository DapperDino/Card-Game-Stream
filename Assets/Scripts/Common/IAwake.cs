namespace CardGame.Common
{
    public interface IAwake { void Awake(); }

    public static class AwakeExtensions
    {
        public static void Awake(this IContainer container)
        {
            foreach (var aspect in container.Aspects)
            {
                if (aspect is IAwake item) { item.Awake(); }
            }
        }
    }
}
