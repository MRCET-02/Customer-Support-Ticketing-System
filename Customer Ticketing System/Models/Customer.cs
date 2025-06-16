namespace Customer_Ticketing_System.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }

        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
