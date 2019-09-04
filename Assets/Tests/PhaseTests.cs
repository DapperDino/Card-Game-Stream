using CardGame.Common;
using CardGame.GameActions;
using NUnit.Framework;
using System.Collections;

namespace Tests
{
    public class PhaseTests
    {
        private class TestAction : GameAction
        {
            public int LoopCount { get; }
            public int Keyframe { get; }
            public int Step { get; private set; }
            public bool IsComplete { get; private set; }

            public TestAction(int loopCount, int keyframe)
            {
                LoopCount = loopCount;
                Keyframe = keyframe;
            }

            public IEnumerator KeyframeViewer(IContainer container, GameAction action)
            {
                for (Step = 0; Step < LoopCount; ++Step)
                {
                    if (Step == Keyframe) { yield return true; }
                    else { yield return false; }
                }
            }

            public void KeyFrameHandler(IContainer container) => IsComplete = true;
        }

        [Test]
        public void RunningPhase_CompletesViewerFlow()
        {
            var action = new TestAction(10, 5);
            var phase = new Phase(action, action.KeyFrameHandler) { Viewer = action.KeyframeViewer };

            var flow = phase.Flow(null);
            while (flow.MoveNext()) { }

            Assert.AreEqual(action.Step, action.LoopCount);
        }

        [Test]
        public void RunningPhase_HandlerIsTriggered()
        {
            var action = new TestAction(10, 5);
            var phase = new Phase(action, action.KeyFrameHandler) { Viewer = action.KeyframeViewer };

            var flow = phase.Flow(null);
            while (flow.MoveNext()) { }

            Assert.IsTrue(action.IsComplete);
        }

        [Test]
        public void RunningPhaseWithoutKeyframe_HandlerIsTriggered()
        {
            var action = new TestAction(10, -1);
            var phase = new Phase(action, action.KeyFrameHandler) { Viewer = action.KeyframeViewer };

            var flow = phase.Flow(null);
            while (flow.MoveNext()) { }

            Assert.IsTrue(action.IsComplete);
        }

        [Test]
        public void RunningPhaseWithKeyframe_HandlerIsTriggeredOnKeyframe()
        {
            var action = new TestAction(10, 5);
            var phase = new Phase(action, action.KeyFrameHandler) { Viewer = action.KeyframeViewer };

            var flow = phase.Flow(null);

            while (flow.MoveNext())
            {
                if (action.Step < action.Keyframe) { Assert.IsFalse(action.IsComplete); }
                else if (action.Step > action.Keyframe) { Assert.IsTrue(action.IsComplete); }
            }
        }
    }
}
