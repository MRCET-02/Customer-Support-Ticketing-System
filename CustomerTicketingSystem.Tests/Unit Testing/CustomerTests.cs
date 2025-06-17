using Customer_Ticketing_System.Models;

namespace Customer_Ticketing_System.Tests
{
    [TestFixture]
    public class CustomerTests
    {
        
        [TestCase("Thanuja", "thanu23@gmail.com")]
        [TestCase("Alice", "alice@gmail.com")]
        [TestCase("Bob", "bob@domain.com")]
        [TestCase("sai", "sai@abc.org")]
        [TestCase("Mahi", "mahi123@.net")]
        public async Task Ctor_SetsNameAndEmail_Async(string name, string email)
        {
            var customer = await Task.Run(() => new Customer(name, email));

            Assert.AreEqual(name, customer.Name);
            Assert.AreEqual(email, customer.Email);
        }

        // Test constructor generates a non-empty CustomerId
        [Test]
        public async Task Ctor_GeneratesCustomerId_Async()
        {
            var customer = await Task.Run(() => new Customer("Ammulu", "ammu12@gmail.com"));

            Assert.AreNotEqual(Guid.Empty, customer.CustomerId);
        }

        [TestCase("Charlie")]
        [TestCase("Charlie Updated")]
        [TestCase("New Name")]
        [TestCase("12345")]
        [TestCase("")]
        public async Task Update_Name_Succeeds_Async(string newName)
        {
            var customer = await Task.Run(() => new Customer("InitialName", "charlie@gmail.com"));
            await Task.Run(() => customer.Name = newName);

            Assert.AreEqual(newName, customer.Name);
        }

        
        [TestCase("dave.updated@gmail.com")]
        [TestCase("test.user@domain.org")]
        [TestCase("person123@abc.net")]
        [TestCase("email@sub.domain.com")]
        [TestCase("another.email@company.co")]
        public async Task Update_Email_Succeeds_Async(string newEmail)
        {
            var customer = await Task.Run(() => new Customer("David", "dave@gmail.com"));
            await Task.Run(() => customer.Email = newEmail);

            Assert.AreEqual(newEmail, customer.Email);
        }
    }
}
