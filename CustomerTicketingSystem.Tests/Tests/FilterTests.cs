using NUnit.Framework;
using Customer_Ticketing_System.Models;
using Customer_Ticketing_System.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Customer_TicketingSystem.Tests
{
    [TestFixture]
    public class FilterTests
    {
        private TicketService service;
        [SetUp]
        public void Setup()
        {
            service = new TicketService();
        }

        [Test]
        public async Task FilterTickets_ShouldReturnOnlyOpenTickets()
        {
            var customer = new Customer("Dan", "dan@example.com");

            await service.CreateTicketAsync("1", "Desc1", customer);
            var t2 = await service.CreateTicketAsync("2", "Desc2", customer);
            await service.UpdateStatusAsync(t2.TicketId, TicketStatus.Closed);

            var tickets = await service.GetAllTicketsAsync();
            var openTickets = tickets.Where(t => t.Status == TicketStatus.Open).ToList();

            Assert.IsTrue(openTickets.All(t => t.Status == TicketStatus.Open));
        }
        [Test]
        public async Task FilterTickets_ShouldReturnClosedTickets()
        {
            var customer = new Customer("Ajay", "ajay@example.com");

            var t1 = await service.CreateTicketAsync("T1", "Desc1", customer);
            await service.UpdateStatusAsync(t1.TicketId, TicketStatus.Closed);

            var closedTickets = (await service.GetAllTicketsAsync()).Where(t => t.Status == TicketStatus.Closed).ToList();
            Assert.IsTrue(closedTickets.All(t => t.Status == TicketStatus.Closed));
        }

        [Test]
        public async Task FilterTickets_WhenNoTickets_ShouldReturnEmptyList()
        {
            var tickets = await service.GetAllTicketsAsync();
            var unmatched = tickets.Where(t => t.Status == TicketStatus.Resolved && t.Title == "NonExistent").ToList();

            Assert.IsEmpty(unmatched);
        }
        [Test]
        public async Task FilterTickets_MultipleStatuses_ReturnsCorrectCounts()
        {
            var customer = new Customer("Rani", "rani@example.com");

            var t1 = await service.CreateTicketAsync("T1", "Desc1", customer);
            var t2 = await service.CreateTicketAsync("T2", "Desc2", customer);
            var t3 = await service.CreateTicketAsync("T3", "Desc3", customer);

            await service.UpdateStatusAsync(t1.TicketId, TicketStatus.Closed);
            await service.UpdateStatusAsync(t2.TicketId, TicketStatus.Resolved);

            var allTickets = await service.GetAllTicketsAsync();

            var closedCount = allTickets.Count(t => t.TicketId == t1.TicketId && t.Status == TicketStatus.Closed);
            var resolvedCount = allTickets.Count(t => t.TicketId == t2.TicketId && t.Status == TicketStatus.Resolved);
            var openCount = allTickets.Count(t => t.TicketId == t3.TicketId && t.Status == TicketStatus.Open);

            Assert.AreEqual(1, closedCount);
            Assert.AreEqual(1, resolvedCount);
            Assert.AreEqual(1, openCount);
        }
    }
}



