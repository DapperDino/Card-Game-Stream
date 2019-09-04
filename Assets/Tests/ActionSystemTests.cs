using CardGame.Common;
using CardGame.Common.Notifications;
using CardGame.GameActions;
using CardGame.Systems;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class ActionSystemTests
    {
        private class TestAction : GameAction
        {
            public bool DidPrepare { get; set; }
            public bool DidPerform { get; set; }
        }

        private class NotificationMarks
        {
            public bool SequenceBegin { get; set; }
            public bool SequenceEnd { get; set; }
            public bool Complete { get; set; }
            public bool Prepare { get; set; }
            public bool Perform { get; set; }
            public bool DeathReaper { get; set; }
        }

        private class TestSystem : Aspect
        {
            private const int RootActionOrder = 0;
            private const int DepthCheckPriority = 1;
            private const int DepthReactionOrder = int.MinValue;

            private readonly NotificationMarks reactionMarks = new NotificationMarks();

            public NotificationMarks ActionMarks { get; } = new NotificationMarks();
            public List<TestAction> Reactions { get; } = new List<TestAction>();
            public bool LoopedDeath { get; private set; } = false;
            public bool DepthFirst { get; private set; } = false;

            public void SetUp()
            {
                this.AddObserver(OnSequenceBegin, ActionSystem.OnBeginSequenceNotification);
                this.AddObserver(OnSequenceEnd, ActionSystem.OnEndSequenceNotification);
                this.AddObserver(OnComplete, ActionSystem.OnCompleteNotification);
                this.AddObserver(OnPrepare, $"{typeof(TestAction).Name}.OnPrepare");
                this.AddObserver(OnPerform, $"{typeof(TestAction).Name}.OnPerform");
                this.AddObserver(OnDeath, ActionSystem.OnDeathReaperNotification);
            }

            public void TearDown()
            {
                this.RemoveObserver(OnSequenceBegin, ActionSystem.OnBeginSequenceNotification);
                this.RemoveObserver(OnSequenceEnd, ActionSystem.OnEndSequenceNotification);
                this.RemoveObserver(OnComplete, ActionSystem.OnCompleteNotification);
                this.RemoveObserver(OnPrepare, $"{typeof(TestAction).Name}.OnPrepare");
                this.RemoveObserver(OnPerform, $"{typeof(TestAction).Name}.OnPerform");
                this.RemoveObserver(OnDeath, ActionSystem.OnDeathReaperNotification);
            }

            private void OnSequenceBegin(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == RootActionOrder ? ActionMarks : reactionMarks;

                marks.SequenceBegin = true;

                action.PreparePhase.Viewer = TestViewer;
                action.PerformPhase.Viewer = TestViewer;
            }

            private void OnSequenceEnd(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == RootActionOrder ? ActionMarks : reactionMarks;

                marks.SequenceEnd = true;
            }

            private void OnComplete(object sender, object args) => ActionMarks.Complete = true;

            private void OnPrepare(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == RootActionOrder ? ActionMarks : reactionMarks;

                marks.Prepare = true;
                action.DidPrepare = true;
            }

            private void OnPerform(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == RootActionOrder ? ActionMarks : reactionMarks;

                marks.Perform = true;
                action.DidPerform = true;

                if (action.OrderOfPlay != RootActionOrder) { Reactions.Add(action); }
                else { AddReactions((IContainer)sender); }

                if (action.Priority == DepthCheckPriority)
                {
                    var reaction = new TestAction { OrderOfPlay = DepthReactionOrder };
                    ((IContainer)sender).GetAspect<ActionSystem>().AddReaction(reaction);
                }

                if (action.OrderOfPlay == DepthReactionOrder) { DepthFirst = (Reactions.Count == 2); }
            }

            private void OnDeath(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == RootActionOrder ? ActionMarks : reactionMarks;

                if (!ActionMarks.DeathReaper)
                {
                    var reaction = new TestAction { OrderOfPlay = int.MaxValue };
                    ((ActionSystem)sender).AddReaction(reaction);
                }
                else { LoopedDeath = true; }

                marks.DeathReaper = true;
            }

            private IEnumerator TestViewer(IContainer container, GameAction action)
            {
                yield return null;
                yield return true;
                yield return null;
            }

            private void AddReactions(IContainer container)
            {
                for (int i = 0; i < 5; ++i)
                {
                    var reaction = new TestAction { OrderOfPlay = Random.Range(1, 100) };

                    if (i == 2) { reaction.Priority = DepthCheckPriority; }

                    container.GetAspect<ActionSystem>().AddReaction(reaction);
                }
            }
        }

        private IContainer game = null;
        private ActionSystem actionSystem = null;
        private TestSystem testSystem = null;

        [SetUp]
        public void TestSetup()
        {
            NotificationCenter.Instance.Clean();

            game = new Container();
            actionSystem = game.AddAspect<ActionSystem>();
            testSystem = game.AddAspect<TestSystem>();
            testSystem.SetUp();
        }

        [TearDown]
        public void TestTearDown() { testSystem.TearDown(); }

        private void RunToCompletion()
        {
            var timeOut = 0;
            while (actionSystem.IsActive && timeOut < 1000)
            {
                timeOut++;
                actionSystem.Update();
            }
        }

        [Test]
        public void ChangingActionState_ActionSystemTracksState()
        {
            actionSystem.Perform(new TestAction());

            Assert.IsTrue(actionSystem.IsActive);

            RunToCompletion();

            Assert.IsFalse(actionSystem.IsActive);
        }

        [Test]
        public void CompletingAction_AllNotificationsAreRaised()
        {
            actionSystem.Perform(new TestAction());
            RunToCompletion();
            var marks = testSystem.ActionMarks;

            bool result = marks.SequenceBegin &&
                marks.SequenceEnd &&
                marks.Complete &&
                marks.Prepare &&
                marks.Perform &&
                marks.DeathReaper;

            Assert.IsTrue(result);
        }

        [Test]
        public void CompletingAction_ReactionsAreSorted()
        {
            actionSystem.Perform(new TestAction());
            RunToCompletion();

            int priority = int.MaxValue;
            int orderOfPlay = int.MinValue;

            for (int i = 0; i < testSystem.Reactions.Count; ++i)
            {
                var reaction = testSystem.Reactions[i];

                Assert.LessOrEqual(reaction.Priority, priority);

                if (reaction.Priority != priority)
                {
                    priority = reaction.Priority;
                    orderOfPlay = int.MinValue;
                }

                Assert.GreaterOrEqual(reaction.OrderOfPlay, orderOfPlay);

                orderOfPlay = reaction.OrderOfPlay;
            }
        }

        [Test]
        public void CancellingAction_ActionDoesNotFinish()
        {
            var action = new TestAction();
            action.Cancel();
            actionSystem.Perform(action);

            RunToCompletion();

            Assert.IsFalse(action.DidPrepare);
            Assert.IsFalse(action.DidPerform);
        }

        [Test]
        public void CompletingAction_DeathIsLooped()
        {
            actionSystem.Perform(new TestAction());

            RunToCompletion();

            Assert.IsTrue(testSystem.LoopedDeath);
        }

        [Test]
        public void CompletingAction_ReactionsAreDepthFirst()
        {
            actionSystem.Perform(new TestAction());

            RunToCompletion();

            Assert.IsTrue(testSystem.DepthFirst);
        }
    }
}
