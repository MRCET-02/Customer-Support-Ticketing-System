using Customer_Ticketing_System.Models;

namespace Customer_Ticketing_System.Tests
{
    [TestFixture]
    public class StatusTests
    {
        [Test]
        public async Task Default_Status_Is_Open_Async()
        {
            var customer = await Task.Run(() => new Customer("Thanuja", "thanu23@gmail.com"));
            var ticket = await Task.Run(() => new Ticket("Title", "Desc", customer));

            Assert.AreEqual(TicketStatus.Open, ticket.Status);
        }

        [TestCase("Agent1", "agent1@gmail.com")]
        [TestCase("Agent2", "agent2@gmail.com")]
        public async Task AssignAgent_Changes_Status_To_InProgress_Async(string agentName, string agentEmail)
        {
            var customer = await Task.Run(() => new Customer("Test User", "test@gmail.com"));
            var agent = await Task.Run(() => new Agent(agentName, agentEmail));
            var ticket = await Task.Run(() => new Ticket("Title", "Desc", customer));

            ticket.AssignAgent(agent);

            Assert.AreEqual(TicketStatus.InProgress, ticket.Status);
            Assert.AreEqual(agent, ticket.AssignedAgent);
        }

        [TestCase(TicketStatus.Resolved)]
        [TestCase(TicketStatus.Closed)]
        [TestCase(TicketStatus.InProgress)]
        [TestCase(TicketStatus.Open)]
        public async Task UpdateStatus_Changes_Status_Async(TicketStatus newStatus)
        {
            var customer = await Task.Run(() => new Customer("TestUser", "test@gmail.com"));
            var ticket = await Task.Run(() => new Ticket("Title", "Desc", customer));

            ticket.UpdateStatus(newStatus);

            Assert.AreEqual(newStatus, ticket.Status);
        }

        [TestCase(TicketStatus.Open, TicketStatus.InProgress)]
        [TestCase(TicketStatus.InProgress, TicketStatus.Resolved)]
        [TestCase(TicketStatus.Resolved, TicketStatus.Closed)]
        public async Task UpdateStatus_Transitions_Async(TicketStatus fromStatus, TicketStatus toStatus)
        {
            var customer = await Task.Run(() => new Customer("Test User", "test@gmail.com"));
            var ticket = await Task.Run(() => new Ticket("Title", "Desc", customer));

            ticket.UpdateStatus(fromStatus);
            ticket.UpdateStatus(toStatus);

            Assert.AreEqual(toStatus, ticket.Status);
        }
    }
}
