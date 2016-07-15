using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Diagnostics
{
    [TestFixtureFor(typeof (TestTraceListener))]
    // ReSharper disable once InconsistentNaming
    internal class TestTraceListener_Should
    {
        [Test]
        [Issue("#16")]
        public void Be_Public()
        {
            typeof (TestTraceListener).IsPublic.Should().BeTrue();
        }

        [Test]
        public void Be_Creatable()
        {
            // lets count the before
            var listenerCount = Trace.Listeners.Count;

            using (var ttl = new TestTraceListener())
            {
                Trace.Listeners.Count.Should().Be(listenerCount + 1);

                Trace.TraceError("here is a message");
                ttl.MessagesFor(TraceLevel.Error).Should().Contain("here is a message");
                ttl.MessagesFor(TraceLevel.Info).Should().NotContain("here is a message");
            }

            // see if it got deregistered
            Trace.Listeners.Count.Should().Be(listenerCount);
        }

        [Test]
        public void Handle_Error_Warn_Information()
        {
            using (var ttl = new TestTraceListener())
            {
                Trace.TraceError("cool error");
                Trace.TraceWarning("awesome warning");
                Trace.TraceInformation("great information");
                Trace.WriteLine("nice debug");

                ttl.MessagesFor(TraceLevel.Error).Should().Contain("cool error");
                ttl.MessagesFor(TraceLevel.Warning).Should().Contain("awesome warning");
                ttl.MessagesFor(TraceLevel.Info).Should().Contain("great information");
                ttl.MessagesFor(TraceLevel.Verbose).Should().BeEmpty(because: "level is set to info by default");
            }
        }

        [Test]
        public void Handle_Debug()
        {
            using (var ttl = new TestTraceListener())
            {
                ttl.ActiveTraceLevel = TraceLevel.Verbose;
                Trace.WriteLine("nice debug");
                ttl.MessagesFor(TraceLevel.Verbose).Should().Contain("nice debug");
            }
        }

        [Test]
        public void Handle_Debug_Using_Debug_Class()
        {
            using (var ttl = new TestTraceListener())
            {
                ttl.ActiveTraceLevel = TraceLevel.Verbose;
                Debug.WriteLine("nice debug");

#if DEBUG
                ttl.MessagesFor(TraceLevel.Verbose).Should().Contain("nice debug");
#else
                ttl.MessagesFor(TraceLevel.Verbose).Should().BeEmpty();
#endif
            }
        }

        [Test]
        public void Support_clearing_messages()
        {
            using (var ttl = new TestTraceListener())
            {
                Trace.TraceInformation("nice info");
                ttl.MessagesFor(TraceLevel.Info).Single().Should().Be("nice info");

                ttl.ClearMessagesFor(TraceLevel.Info);
                ttl.MessagesFor(TraceLevel.Info).Should().BeEmpty();

                Trace.TraceError("not so nice error");
                ttl.ClearMessages();
                ttl.MessagesFor(TraceLevel.Error).Should().BeEmpty();

            }
        }
    }
}
