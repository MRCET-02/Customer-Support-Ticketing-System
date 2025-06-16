using Customer_Ticketing_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Ticketing_System.Classes
{

    public class TicketClass
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public TicketStatus Status { get; set; }
        public PriorityLevel Priority { get; set; }
        public AgentClass AgentAssigned { get; set; }
        public CustomerClass Customer { get; set; }
    }
    public class TicketOperation
    {
        public int _nextTicketId = 0;
        public List<TicketClass> Tickets = new List<TicketClass>();
        //Adding Ticket
        public TicketClass CreateTicket(string title, PriorityLevel priority, CustomerClass customer, AgentClass agent)
        {
            var newTicket = new TicketClass
            {
                Id = _nextTicketId++,
                Title = title,
                Priority = priority,
                Customer = customer,
                AgentAssigned = agent,
                Status = TicketStatus.Open,
                DateCreated = DateTime.UtcNow
            };

            Tickets.Add(newTicket);
            Console.WriteLine($"Ticket {newTicket.Id} created successfully.");
            return newTicket;
        }
        //Get All Tickets
        public List<TicketClass> GetAllTickets()
        {
            return Tickets;
        }

        //Update Ticket 
        public void UpdateTicket(TicketClass updatedticket)
        {
            try
            {
                foreach (var t in Tickets)
                {
                    if (t.Id == updatedticket.Id)
                    {
                        if (t.Status == TicketStatus.Closed)
                        {
                            Console.WriteLine("Cannot Update this Ticket as this Ticket is Closed");
                        }
                        else if (t.Customer != updatedticket.Customer)
                        {
                            Console.WriteLine($"{updatedticket.Customer.Name} Doest Have the Access to Update this Ticket");
                        }

                        else
                        {
                            t.Status = updatedticket.Status;
                            t.Priority = updatedticket.Priority;
                            t.DateUpdated = DateTime.Now;
                            Console.WriteLine($"Ticket {updatedticket.Id} has been updated. New status: {t.Status}.");
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        //Closure Of Ticket
        public void TicketClosure(TicketClass ticket)
        {
            try
            {

                //Check if Ticket with ID exist
                foreach (var t in Tickets)
                {
                    if (t.Id == ticket.Id)
                    {
                        Console.WriteLine($"Ticket with ID already Exist and the Progress is {ticket.Status}");
                    }
                }
                //Delete Ticket from the List
                ticket.Status = TicketStatus.Closed;
                ticket.DateUpdated = DateTime.UtcNow;
                Console.WriteLine($"Ticket is Successfully added. Your Ticket Agent is {ticket.AgentAssigned.AgentName}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        //Filter By Status
        public List<TicketClass> GetTicketsByStatus(TicketStatus status)
        {
            return Tickets.Where(e => e.Status == status).ToList();
        }
        //Filter By Priority
        public List<TicketClass> GetTicketsByPriority(PriorityLevel priorityLevel)
        {
            return Tickets.Where(e => e.Priority == priorityLevel).ToList();
        }
        //Filter By Customer 
        public List<TicketClass> GetTicketsByCustomers(int CId)
        {
            return Tickets.Where(e => e.Customer.Id == CId).ToList();
        }
        //Filter By Time Range 
        public List<TicketClass> GetTicketsByTime(DateTime date)
        {
            return Tickets.Where(e => e.DateCreated == date || e.DateUpdated == date).ToList();
        }
    }
}
