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
        private static readonly string logFilePath;

        static TicketService()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(baseDirectory, "Data", "Logs");
            Directory.CreateDirectory(dataDirectory);
            logFilePath = Path.Combine(dataDirectory, "error.log");
        }

        private async Task LogErrorAsync(Exception ex)
        {
            var errorMessage = $"[{DateTime.Now}] {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}\n\n";
            await File.AppendAllTextAsync(logFilePath, errorMessage);
        }

        public async Task<Ticket> CreateTicketAsync(string title, string description, Customer customer, DateTime? createdAt = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
                {
                    throw new ArgumentException("Title and description are required fields and cannot be empty.");
                }
                if (customer == null)
                {
                    throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
                }

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

        public async Task AssignAgentAsync(Guid ticketId, Agent agent)
        {
            try
            {
                if (agent == null)
                {
                    throw new ArgumentNullException(nameof(agent), "Agent cannot be null.");
                }

                var tickets = await TicketFileHandler.ReadTicketsAsync();
                var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);

                if (ticket == null)
                {
                    throw new KeyNotFoundException($"Ticket with ID '{ticketId}' was not found.");
                }

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
                var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);

                if (ticket == null)
                {
                    throw new KeyNotFoundException($"Ticket with ID '{ticketId}' was not found.");
                }
                if(ticket.Status == TicketStatus.Closed)
                {
                    throw new Exception("Cannot Updated a Closed Ticket Status");
                }
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
                var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);

                if (ticket == null)
                {
                    throw new KeyNotFoundException($"Ticket with ID '{ticketId}' was not found.");
                }
                if (ticket.Status == TicketStatus.Closed)
                {
                    throw new Exception("Cannot Close an Already Closed Ticket");
                }
                else
                {
                    ticket.Status = TicketStatus.Closed;
                    ticket.UpdatedAt = DateTime.UtcNow;
                }
                    await TicketFileHandler.WriteTicketsAsync(tickets);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }
        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            try
            {
                return await TicketFileHandler.ReadTicketsAsync();
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex);
                throw;
            }
        }

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
                    .Where(t => t.Customer != null && t.Customer.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
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
                    .Where(t => t.AssignedAgent?.Department != null && t.AssignedAgent.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
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