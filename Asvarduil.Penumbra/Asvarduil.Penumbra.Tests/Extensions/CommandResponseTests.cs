using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asvarduil.Penumbra.StarMadeCore;

namespace Asvarduil.Penumbra.Tests.Extensions
{
    [TestClass]
    public class CommandResponseTests
    {
        [TestMethod]
        [TestCategory("Command Responses")]
        public void AddMessageCommandSendsExpectedResults()
        {
            var commands = new List<string>();
            commands.AddMessageCommand("UnitTest", "Test Message");

            string expected = "/server_message_to info UnitTest \"Test Message\"";
            Assert.AreEqual(commands[0], expected);
        }

        [TestMethod]
        [TestCategory("Command Responses")]
        public void AddBroadcastCommandSendsExpectedResults()
        {
            var commands = new List<string>();
            commands.AddBroadcastCommand("Test Message");

            string expected = "/server_message_broadcast info \"Test Message\"";
            Assert.AreEqual(commands[0], expected);
        }
    }
}
