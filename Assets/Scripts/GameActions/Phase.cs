using CardGame.Common;
using System;
using System.Collections;

namespace CardGame.GameActions
{
    public class Phase
    {
        public GameAction Owner { get; }
        public Action<IContainer> Handler { get; }
        public Func<IContainer, GameAction, IEnumerator> Viewer { get; set; }

        public Phase(GameAction owner, Action<IContainer> handler)
        {
            Owner = owner;
            Handler = handler;
        }

        public IEnumerator Flow(IContainer container)
        {
            bool hitKeyFrame = false;

            if (Viewer != null)
            {
                var sequence = Viewer(container, Owner);

                while (sequence.MoveNext())
                {
                    bool isKeyFrame = (sequence.Current is bool) ? (bool)sequence.Current : false;

                    if (isKeyFrame)
                    {
                        hitKeyFrame = true;
                        Handler?.Invoke(container);
                    }

                    yield return null;
                }
            }

            if (!hitKeyFrame) { Handler?.Invoke(container); }
        }
    }
}
