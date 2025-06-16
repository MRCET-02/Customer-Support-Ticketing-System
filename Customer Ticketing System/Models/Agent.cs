namespace Customer_Ticketing_System.Models
{
    public class Agent
    {
        public Guid AgentId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Department { get; set; }

        public Agent(string name, string department)
        {
            Name = name;
            Department = department;
        }
    }
}
