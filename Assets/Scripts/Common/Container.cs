using System.Collections.Generic;

namespace CardGame.Common
{
    public class Container : IContainer
    {
        private readonly Dictionary<string, IAspect> aspects = new Dictionary<string, IAspect>();

        public ICollection<IAspect> Aspects => aspects.Values;

        public T AddAspect<T>(string key = null) where T : IAspect, new() => AddAspect(new T(), key);

        public T AddAspect<T>(T aspect, string key = null) where T : IAspect
        {
            key = key ?? typeof(T).Name;
            aspects.Add(key, aspect);
            aspect.Container = this;
            return aspect;
        }

        public T GetAspect<T>(string key = null) where T : IAspect
        {
            key = key ?? typeof(T).Name;
            T aspect = aspects.ContainsKey(key) ? (T)aspects[key] : default;
            return aspect;
        }
    }
}
