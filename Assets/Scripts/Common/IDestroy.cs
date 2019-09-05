namespace CardGame.Common
{
    public interface IDestroy { void Destroy(); }

    public static class DestroyExtensions
    {
        public static void Destroy(this IContainer container)
        {
            foreach (var aspect in container.Aspects)
            {
                if (aspect is IDestroy item) { item.Destroy(); }
            }
        }
    }
}
