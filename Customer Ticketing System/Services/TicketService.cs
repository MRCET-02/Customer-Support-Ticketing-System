using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Customer_Ticketing_System.Models;

namespace Customer_Ticketing_System.Services
{
    public class TicketService
    {
        // ─────────────────────────────  Logging  ─────────────────────────────
        private static readonly string logFilePath;

        static TicketService()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(baseDirectory, "Data", "Logs");
            Directory.CreateDirectory(dataDirectory);

            logFilePath = Path.Combine(dataDirectory, "error.log");
        }

        private static async Task LogErrorAsync(Exception ex)
        {
            string entry =
                $"[{DateTime.Now}] {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{Environment.NewLine}";
            await File.AppendAllTextAsync(logFilePath, entry);
        }

        // ─────────────────────────────  Create  ──────────────────────────────
        public async Task<Ticket> CreateTicketAsync(
            string title,
            string description,
            Customer customer,
            DateTime? createdAt = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
                    throw new ArgumentException("Title and description cannot be empty.");

                if (customer is null)
                    throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");

                var tickets = await TicketFileHandler.ReadTicketsAsync();

                var ticket = new Ticket(title, description, customer)
                {
                    CreatedAt = createdAt ?? DateTime.UtcNow
                };

                tickets.Add(ticket);
                await TicketFileHandler.WriteTicketsAsync(tickets);
                return ticket;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }

        // ─────────────────────────────  Update  ──────────────────────────────
        public async Task AssignAgentAsync(Guid ticketId, Agent agent)
        {
            try
            {
                if (agent is null)
                    throw new ArgumentNullException(nameof(agent), "Agent cannot be null.");

                var tickets = await TicketFileHandler.ReadTicketsAsync();
                var ticket  = tickets.FirstOrDefault(t => t.TicketId == ticketId);

                if (ticket is null)
                    throw new KeyNotFoundException($"Ticket '{ticketId}' not found.");

                ticket.AssignAgent(agent);
                await TicketFileHandler.WriteTicketsAsync(tickets);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }

        public async Task UpdateStatusAsync(Guid ticketId, TicketStatus status)
        {
            try
            {
                var tickets = await TicketFileHandler.ReadTicketsAsync();
                var ticket  = tickets.FirstOrDefault(t => t.TicketId == ticketId);

                if (ticket is null)
                    throw new KeyNotFoundException($"Ticket '{ticketId}' not found.");

                if (ticket.Status == TicketStatus.Closed)
                    throw new InvalidOperationException("Cannot update a closed ticket.");

                ticket.UpdateStatus(status);
                ticket.UpdatedAt = DateTime.UtcNow;

                await TicketFileHandler.WriteTicketsAsync(tickets);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }

        public async Task CloseTicketAsync(Guid ticketId)
        {
            try
            {
                var tickets = await TicketFileHandler.ReadTicketsAsync();
                var ticket  = tickets.FirstOrDefault(t => t.TicketId == ticketId);

                if (ticket is null)
                    throw new KeyNotFoundException($"Ticket '{ticketId}' not found.");

                if (ticket.Status == TicketStatus.Closed)
                    throw new InvalidOperationException("Ticket is already closed.");

                ticket.Status    = TicketStatus.Closed;
                ticket.UpdatedAt = DateTime.UtcNow;

                await TicketFileHandler.WriteTicketsAsync(tickets);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }

        // ─────────────────────────────  Queries  ─────────────────────────────
        public async Task<List<Ticket>> GetAllTicketsAsync() =>
            await TicketFileHandler.ReadTicketsAsync();

        public async Task<List<Ticket>> GetByStatusAsync(TicketStatus status)
        {
            try
            {
                var tickets = await TicketFileHandler.ReadTicketsAsync();
                return tickets.Where(t => t.Status == status).ToList();
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }

        public async Task<List<Ticket>> GetByCustomerAsync(string name)
        {
            try
            {
                var tickets = await TicketFileHandler.ReadTicketsAsync();
                return tickets
                    .Where(t => t.Customer?.Name.Equals(name, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }

        public async Task<List<Ticket>> GetByAgentDepartmentAsync(string department)
        {
            try
            {
                var tickets = await TicketFileHandler.ReadTicketsAsync();
                return tickets
                    .Where(t => t.AssignedAgent?.Department?.Equals(department, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }

        public async Task<List<Ticket>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                var tickets = await TicketFileHandler.ReadTicketsAsync();
                return tickets
                    .Where(t => t.CreatedAt >= from && t.CreatedAt <= to)
                    .ToList();
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }
    }
}
