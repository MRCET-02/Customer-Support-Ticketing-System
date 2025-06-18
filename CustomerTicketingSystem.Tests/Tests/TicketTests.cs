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
        [Test]
        public async Task Ticket_ShouldHaveValidCreatedAt()
        {
            var customer = new Customer("Riya", "riya@example.com");
            var ticket = await service.CreateTicketAsync("Date Check", "Testing date", customer);

            Assert.LessOrEqual(ticket.CreatedAt, DateTime.Now);
        }

        [Test]
        public async Task Ticket_ShouldHaveDefaultStatusOpen()
        {
            var customer = new Customer("Asha", "asha@example.com");
            var ticket = await service.CreateTicketAsync("Default Status", "Check status", customer);

            Assert.AreEqual(TicketStatus.Open, ticket.Status);
        }

        [Test]
        public void CreateTicket_WithEmptyDescription_ShouldThrow()
        {
            var customer = new Customer("Sam", "sam@example.com");

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.CreateTicketAsync("Valid Title", "", customer);
            });
        }

        [Test]
        public async Task CreateTicket_TitleShouldNotBeNullOrEmpty()
        {
            var customer = new Customer("Neha", "neha@example.com");
            var ticket = await service.CreateTicketAsync("Support Needed", "Desc", customer);

            Assert.IsFalse(string.IsNullOrWhiteSpace(ticket.Title));
        }

    }
}

