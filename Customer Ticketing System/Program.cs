using System;
using Customer_Ticketing_System.Models;
using Customer_Ticketing_System.Services;

namespace CustomerTicketingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var cust1 = new Customer("Bharath", "bharath@email.com");
            var cust2 = new Customer("Anita", "anita@email.com");
            var agent1 = new Agent("Deepa", "Support");
            var agent2 = new Agent("Rajesh", "Escalations");

            // ticket list
            var tickets = new List<Ticket>

            {
                new Ticket("Login Issue",     "Cannot log in",     cust1) { CreatedAt = DateTime.Today.AddDays(-5) },
                new Ticket("Payment Failure", "Error on checkout",  cust2) { CreatedAt = DateTime.Today.AddDays(-2) },
                new Ticket("Bug in UI",       "Misaligned button",  cust1) { CreatedAt = DateTime.Today.AddDays(-1) }
            };

            // Assign agents & update statuses
            tickets[0].AssignAgent(agent1);                        // now InProgress
            tickets[1].AssignAgent(agent2); tickets[1].UpdateStatus(TicketStatus.Resolved);

            // Instantiate service
            var svc = new TicketService(tickets);

            // 5) Example: fetch all Open tickets
            var openTickets = svc.GetByStatus(TicketStatus.Open);
            Console.WriteLine("Open Tickets:");
            foreach (var t in openTickets)
                Console.WriteLine($"  [{t.TicketId}] {t.Title} – {t.Customer.Name}");

            // 6) Example: fetch tickets in last 3 days
            var recent = svc.GetByDateRange(DateTime.Today.AddDays(-3), DateTime.Today);
            Console.WriteLine("\nRecent Tickets:");
            foreach (var t in recent)
                Console.WriteLine($"  {t.Title} ({t.CreatedAt:d})");

        }
    }
}
