using CardGame.Common;
using Moq;
using NUnit.Framework;

namespace Tests
{
    public class ContainerTests
    {
        private class TestAspect : IAspect { public IContainer Game { get; set; } }
        private class AltTestAspect : IAspect { public IContainer Game { get; set; } }

        [Test]
        public void AddingAspectToContainer_AspectIsAdded()
        {
            var container = new Container();

            container.AddAspect<TestAspect>();

            Assert.AreEqual(container.Aspects.Count, 1);
        }

        [Test]
        public void AddingMultipleInstancesOfTheSameAspectToContainer_AspectsAreAllAdded()
        {
            var container = new Container();

            container.AddAspect<TestAspect>("Test1");
            container.AddAspect<TestAspect>("Test2");

            Assert.AreEqual(container.Aspects.Count, 2);
        }

        [Test]
        public void AddingMultipleTypesOfAspectToContainer_AspectsAreAllAdded()
        {
            var container = new Container();

            container.AddAspect<TestAspect>();
            container.AddAspect<AltTestAspect>();

            Assert.AreEqual(container.Aspects.Count, 2);
        }

        [Test]
        public void GettingAspectWithoutKey_ReturnsCorrectAspect()
        {
            var container = new Container();

            var original = container.AddAspect<TestAspect>();
            var fetch = container.GetAspect<TestAspect>();

            Assert.AreSame(original, fetch);
        }

        [Test]
        public void GettingAspectWithKey_ReturnsCorrectAspect()
        {
            var container = new Container();

            var original = container.AddAspect<TestAspect>("Test");
            var fetch = container.GetAspect<TestAspect>("Test");

            Assert.AreSame(original, fetch);
        }

        [Test]
        public void GettingMissingAspect_ReturnsNull()
        {
            var container = new Container();

            var fetch = container.GetAspect<TestAspect>("Test");

            Assert.IsNull(fetch);
        }

        [Test]
        public void AddingPreCreatedAspect_AspectIsAdded()
        {
            var container = new Container();
            var aspect = new Mock<IAspect>().Object;

            container.AddAspect(aspect);

            Assert.IsNotEmpty(container.Aspects);
        }

        [Test]
        public void GettingPreCreatedAspect_ReturnsPreCreatedAspect()
        {
            var container = new Container();
            var original = new Mock<IAspect>().Object;

            container.AddAspect(original);
            var fetch = container.GetAspect<IAspect>();

            Assert.AreSame(original, fetch);
        }
    }
}
