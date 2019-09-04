using System.Collections.Generic;

namespace CardGame.Common
{
    public interface IContainer
    {
        ICollection<IAspect> Aspects { get; }
        T AddAspect<T>(string key = null) where T : IAspect, new();
        T AddAspect<T>(T aspect, string key = null) where T : IAspect;
        T GetAspect<T>(string key = null) where T : IAspect;
    }
}