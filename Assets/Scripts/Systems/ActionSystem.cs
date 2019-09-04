using CardGame.Common;
using CardGame.Common.Notifications;
using CardGame.GameActions;
using CardGame.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CardGame.Systems
{
    public class ActionSystem : Aspect
    {
        public const string OnBeginSequenceNotification = "ActionSystem.OnBeginSequence";
        public const string OnEndSequenceNotification = "ActionSystem.OnEndSequence";
        public const string OnDeathReaperNotification = "ActionSystem.OnDeathReaper";
        public const string OnCompleteNotification = "ActionSystem.OnComplete";

        private GameAction rootAction = null;
        private IEnumerator rootSequence = null;
        private List<GameAction> openReactions = null;
        public bool IsActive => rootSequence != null;

        public void Perform(GameAction action)
        {
            if (IsActive) { return; }

            rootAction = action;
            rootSequence = Sequence(action);
        }

        public void Update()
        {
            if (rootSequence == null) { return; }

            if (rootSequence.MoveNext() == true) { return; }

            rootAction = null;
            rootSequence = null;
            openReactions = null;

            this.PostNotification(OnCompleteNotification);
        }

        public void AddReaction(GameAction action)
        {
            if (openReactions == null) { return; }

            openReactions.Add(action);
        }

        private IEnumerator Sequence(GameAction action)
        {
            this.PostNotification(OnBeginSequenceNotification, action);

            if (!action.Validate()) { action.Cancel(); }

            var phase = MainPhase(action.PreparePhase);
            while (phase.MoveNext()) { yield return null; }

            phase = MainPhase(action.PerformPhase);
            while (phase.MoveNext()) { yield return null; }

            phase = MainPhase(action.CancelPhase);
            while (phase.MoveNext()) { yield return null; }

            if (rootAction == action)
            {
                phase = EventPhase(OnDeathReaperNotification, action, true);
                while (phase.MoveNext()) { yield return null; }
            }

            this.PostNotification(OnEndSequenceNotification, action);
        }

        private IEnumerator MainPhase(Phase phase)
        {
            bool isActionCancelled = phase.Owner.IsCanceled;
            bool isCancelPhase = phase.Owner.CancelPhase == phase;

            if (isActionCancelled ^ isCancelPhase) { yield break; }

            var reactions = openReactions = new List<GameAction>();
            var flow = phase.Flow(Container);
            while (flow.MoveNext()) { yield return null; }

            flow = ReactPhase(reactions);
            while (flow.MoveNext()) { yield return null; }
        }

        private IEnumerator ReactPhase(List<GameAction> reactions)
        {
            reactions.Sort(SortActions);

            foreach (var reaction in reactions)
            {
                IEnumerator subFlow = Sequence(reaction);

                while (subFlow.MoveNext()) { yield return null; }
            }
        }

        private IEnumerator EventPhase(string notificationName, GameAction action, bool repeats = false)
        {
            var reactions = new List<GameAction>();
            openReactions = new List<GameAction>();

            do
            {
                reactions = openReactions = new List<GameAction>();
                this.PostNotification(notificationName, action);

                var phase = ReactPhase(reactions);
                while (phase.MoveNext()) { yield return null; }
            }
            while (repeats == true && reactions.Count > 0);
        }

        private int SortActions(GameAction a, GameAction b)
        {
            if (a.Priority != b.Priority)
            {
                return b.Priority.CompareTo(a.Priority);
            }
            else
            {
                return a.OrderOfPlay.CompareTo(b.OrderOfPlay);
            }
        }
    }

    public static class ActionSystemExtensions
    {
        public static void Perform(this IContainer container, GameAction action)
        {
            var actionSystem = container.GetAspect<ActionSystem>();

            if (actionSystem == null) { return; }

            actionSystem.Perform(action);
        }

        public static void AddReaction(this IContainer container, GameAction action)
        {
            var actionSystem = container.GetAspect<ActionSystem>();

            if (actionSystem == null) { return; }

            actionSystem.AddReaction(action);
        }
    }
}
