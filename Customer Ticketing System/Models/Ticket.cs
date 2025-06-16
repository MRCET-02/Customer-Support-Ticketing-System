namespace Customer_Ticketing_System.Models
{
    public class Ticket
    {
        public Guid TicketId { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public Customer Customer { get; set; }
        public Agent? AssignedAgent { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Ticket(string title, string description, Customer customer)
        {
            Title = title;
            Description = description;
            Customer = customer;
        }

        public void AssignAgent(Agent agent)
        {
            AssignedAgent = agent;
            Status = TicketStatus.InProgress;
        }

        public void UpdateStatus(TicketStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
