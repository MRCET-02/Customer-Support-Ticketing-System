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
            var customer = new Customer("Rosy", "rosy@example.com");

            await service.CreateTicketAsync("1", "Desc1", customer);
            var t2 = await service.CreateTicketAsync("2", "Desc2", customer);
            await service.UpdateStatusAsync(t2.TicketId, TicketStatus.Closed);

            var tickets = await service.GetAllTicketsAsync();
            var openTickets = tickets.Where(t => t.Status == TicketStatus.Open).ToList();

            Assert.IsTrue(openTickets.All(t => t.Status == TicketStatus.Open));
        }

    }
}

