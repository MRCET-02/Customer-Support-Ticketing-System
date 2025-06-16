using Customer_Ticketing_System.Models;

namespace Customer_Ticketing_System.Services
{
    public class TicketService
    {
        public async Task<Ticket> CreateTicketAsync(string title, string description, Customer customer)
        {
            var tickets = await TicketFileHandler.ReadTicketsAsync();
            var ticket = new Ticket(title, description, customer);
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
    }
}

