using System;
using System.Collections.Generic;
using System.Linq;
using Customer_Ticketing_System.Models;

namespace Customer_Ticketing_System.Services
{
    public class TicketService
    {
        private readonly List<Ticket> _tickets;
        public TicketService(List<Ticket> tickets) => _tickets = tickets;

        public IEnumerable<Ticket> GetByStatus(TicketStatus status) =>
            _tickets.Where(t => t.Status == status);

        public IEnumerable<Ticket> GetByCustomer(string name) =>
            _tickets.Where(t =>
                t.Customer.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Ticket> GetByAgentDepartment(string dept) =>
            _tickets.Where(t => t.AssignedAgent?.Department == dept);

        public IEnumerable<Ticket> GetByDateRange(DateTime from, DateTime to) =>
            _tickets.Where(t => t.CreatedAt >= from && t.CreatedAt <= to);
    }
}
