using NUnit.Framework;
using Customer_Ticketing_System.Models;
using Customer_Ticketing_System.Services;
using System.Threading.Tasks;

namespace Customer_Ticketing_System.Tests
{
    [TestFixture]
    public class UpdateTests
    {
        private TicketService service;

        [SetUp]
        public void Setup()
        {
            service = new TicketService();
        }

        [Test]
        public async Task AssignAgent_ShouldChangeStatusToInProgress()
        {
            var customer = new Customer("Bob", "bob@example.com");
            var agent = new Agent("AgentX", "Support");
            var ticket = await service.CreateTicketAsync("Bug", "UI fails", customer);

            await service.AssignAgentAsync(ticket.TicketId, agent);

            var updated = (await service.GetAllTicketsAsync()).Find(t => t.TicketId == ticket.TicketId);

            Assert.IsNotNull(updated.AssignedAgent);
            Assert.AreEqual(TicketStatus.InProgress, updated.Status);
        }

        [Test]
        public async Task UpdateStatus_ShouldChangeStatusToClosed()
        {
            var customer = new Customer("Carol", "carol@example.com");
            var ticket = await service.CreateTicketAsync("Crash", "App crashes", customer);

            await service.UpdateStatusAsync(ticket.TicketId, TicketStatus.Closed);

            var updated = (await service.GetAllTicketsAsync()).Find(t => t.TicketId == ticket.TicketId);

            Assert.AreEqual(TicketStatus.Closed, updated.Status);
        }
        [Test]
        public async Task UpdateStatus_ShouldChangeStatusToResolved()
        {
            var customer = new Customer("Frank", "frank@example.com");
            var ticket = await service.CreateTicketAsync("Connection Issue", "Can't connect to Wi-Fi", customer);

            await service.UpdateStatusAsync(ticket.TicketId, TicketStatus.Resolved);

            var updated = (await service.GetAllTicketsAsync()).Find(t => t.TicketId == ticket.TicketId);

            Assert.AreEqual(TicketStatus.Resolved, updated.Status);
        }
        [Test]
        public async Task AssignAgent_Twice_ShouldUpdateToNewAgent()
        {
            var customer = new Customer("Divya", "divya@example.com");
            var agent1 = new Agent("Agent1", "Support");
            var agent2 = new Agent("Agent2", "Escalations");
            var ticket = await service.CreateTicketAsync("Agent Reassignment", "Testing reassignment", customer);

            await service.AssignAgentAsync(ticket.TicketId, agent1);
            await service.AssignAgentAsync(ticket.TicketId, agent2);

            var updated = (await service.GetAllTicketsAsync()).Find(t => t.TicketId == ticket.TicketId);
            Assert.AreEqual("Agent2", updated.AssignedAgent.Name);
        }
        [Test]
        public void UpdateStatus_NonExistentTicket_ShouldThrowKeyNotFound()
        {
            var randomId = Guid.NewGuid();

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.UpdateStatusAsync(randomId, TicketStatus.Closed);
            });
        }

    }
}

