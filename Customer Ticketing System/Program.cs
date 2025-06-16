using System;
using Customer_Ticketing_System.Models;
using Customer_Ticketing_System.Services;

namespace CustomerTicketingSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var customer = new Customer("Bharath", "bharath@email.com");
            var agent = new Agent("Deepa", "Support");

            var ticketService = new TicketService();

            var ticket = await ticketService.CreateTicketAsync("Login Issue", "User cannot log into the portal", customer);
            await ticketService.AssignAgentAsync(ticket.TicketId, agent);
            await ticketService.UpdateStatusAsync(ticket.TicketId, TicketStatus.Resolved);

            Console.WriteLine("Ticket Summary:");
            Console.WriteLine($"Title: {ticket.Title}");
            Console.WriteLine($"Status: {ticket.Status}");
            Console.WriteLine($"Customer: {ticket.Customer.Name}");
            Console.WriteLine($"Agent: {ticket.AssignedAgent?.Name ?? "Unassigned"}");
            Console.WriteLine($"Created At: {ticket.CreatedAt}");
        }
    }
}
