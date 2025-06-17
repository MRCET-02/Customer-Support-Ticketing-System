using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customer_Ticketing_System.Models;


namespace Customer_Ticketing_System.Services
{
    public class TicketService
    {
        public async Task<Ticket> CreateTicketAsync(string title, string description, Customer customer, DateTime? createdAt = null)
        {
            var tickets = await TicketFileHandler.ReadTicketsAsync();
            var ticket = new Ticket(title, description, customer)
            {
                CreatedAt = createdAt ?? DateTime.UtcNow
            };
            tickets.Add(ticket);
            await TicketFileHandler.WriteTicketsAsync(tickets);
            return ticket;
        }

        public async Task AssignAgentAsync(Guid ticketId, Agent agent)
        {
            var tickets = await TicketFileHandler.ReadTicketsAsync();
            var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                ticket.AssignAgent(agent);
                await TicketFileHandler.WriteTicketsAsync(tickets);
            }
        }

        public async Task UpdateStatusAsync(Guid ticketId, TicketStatus status)
        {
            var tickets = await TicketFileHandler.ReadTicketsAsync();
            var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                ticket.UpdateStatus(status);
                await TicketFileHandler.WriteTicketsAsync(tickets);
            }
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await TicketFileHandler.ReadTicketsAsync();
        }

        public async Task<List<Ticket>> GetByStatusAsync(TicketStatus status)
        {
            var tickets = await TicketFileHandler.ReadTicketsAsync();
            return tickets.Where(t => t.Status == status).ToList();
        }

        public async Task<List<Ticket>> GetByCustomerAsync(string name)
        {
            var tickets = await TicketFileHandler.ReadTicketsAsync();
            return tickets
                .Where(t => t.Customer.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<List<Ticket>> GetByAgentDepartmentAsync(string department)
        {
            var tickets = await TicketFileHandler.ReadTicketsAsync();
            return tickets
                .Where(t => t.AssignedAgent?.Department == department)
                .ToList();
        }

        public async Task<List<Ticket>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            var tickets = await TicketFileHandler.ReadTicketsAsync();
            return tickets
                .Where(t => t.CreatedAt >= from && t.CreatedAt <= to)
                .ToList();
        }
    }
}
