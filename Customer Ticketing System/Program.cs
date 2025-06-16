using System;
using Customer_Ticketing_System.Models;

namespace CustomerTicketingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var customer = new Customer("Bharath", "bharath@email.com");
            var agent = new Agent("Deepa", "Support");
            var ticket = new Ticket("Login Issue", "User cannot log into the portal", customer);
            ticket.AssignAgent(agent);
            ticket.UpdateStatus(TicketStatus.Resolved);

            Console.WriteLine("Ticket Summary:");
            Console.WriteLine($"Title: {ticket.Title}");
            Console.WriteLine($"Status: {ticket.Status}");
            Console.WriteLine($"Customer: {ticket.Customer.Name}");
            Console.WriteLine($"Agent: {ticket.AssignedAgent.Name}");
            Console.WriteLine($"Created At: {ticket.CreatedAt}");
        }
    }
}
