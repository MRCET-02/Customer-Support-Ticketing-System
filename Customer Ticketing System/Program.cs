using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customer_Ticketing_System.Models;
using Customer_Ticketing_System.Services;

namespace CustomerTicketingSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cust1 = new Customer("Bharath", "bharath@email.com");
            var cust2 = new Customer("Anita", "anita@email.com");
            var agent1 = new Agent("Deepa", "Support");
            var agent2 = new Agent("Rajesh", "Escalations");

            var service = new TicketService();

            // Create sample tickets
            var t1 = await service.CreateTicketAsync("Login Issue", "Cannot log in", cust1, DateTime.Today.AddDays(-5));
            var t2 = await service.CreateTicketAsync("Payment Failure", "Error on checkout", cust2, DateTime.Today.AddDays(-2));
            var t3 = await service.CreateTicketAsync("Bug in UI", "Misaligned button", cust1, DateTime.Today.AddDays(-1));

            // Assign agents and update status
            await service.AssignAgentAsync(t1.TicketId, agent1);
            await service.AssignAgentAsync(t2.TicketId, agent2);
            await service.UpdateStatusAsync(t2.TicketId, TicketStatus.Resolved);

            // Fetch open tickets
            var openTickets = await service.GetByStatusAsync(TicketStatus.Open);
            Console.WriteLine("Open Tickets:");
            foreach (var t in openTickets)
                Console.WriteLine($"  [{t.TicketId}] {t.Title} – {t.Customer.Name}");

            // Fetch tickets from last 3 days
            var recentTickets = await service.GetByDateRangeAsync(DateTime.Today.AddDays(-3), DateTime.Today);
            Console.WriteLine("\nRecent Tickets:");
            foreach (var t in recentTickets)
                Console.WriteLine($"  {t.Title} ({t.CreatedAt:d})");
        }
    }
}
