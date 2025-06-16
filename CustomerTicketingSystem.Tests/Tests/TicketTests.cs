using NUnit.Framework;
using Customer_Ticketing_System.Models;
using Customer_Ticketing_System.Services;
using System.Threading.Tasks;

namespace Customer_Ticketing_System.Tests
{
    [TestFixture]
    public class TicketTests
    {
        private TicketService service;

        [SetUp]
        public void Setup()
        {
            service = new TicketService();
        }

        [Test]
        public async Task CreateTicket_ShouldCreateNewTicket()
        {
            var customer = new Customer("Alice", "alice@example.com");

            var ticket = await service.CreateTicketAsync("Login Issue", "Cannot login", customer);

            Assert.IsNotNull(ticket);
            Assert.AreEqual("Login Issue", ticket.Title);
            Assert.AreEqual(customer.Name, ticket.Customer.Name);
            Assert.AreEqual(TicketStatus.Open, ticket.Status);
        }
    }
}

