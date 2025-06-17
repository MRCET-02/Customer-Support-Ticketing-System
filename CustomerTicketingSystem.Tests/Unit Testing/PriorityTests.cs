using Customer_Ticketing_System.Models;

namespace Customer_Ticketing_System.Tests
{
    [TestFixture]
    public class PriorityTests
    {
        // Enum values have fixed values, no testcases needed here
        [Test]
        public async Task Enum_Values_Are_Correct_Async()
        {
            await Task.Run(() =>
            {
                Assert.AreEqual(0, (int)Priority.Low);
                Assert.AreEqual(1, (int)Priority.Medium);
                Assert.AreEqual(2, (int)Priority.High);
                Assert.AreEqual(3, (int)Priority.Critical);
            });
        }

        // Assign priority to ticket test - no multiple inputs needed
        [Test]
        public async Task Can_Assign_Priority_To_Ticket_Async()
        {
            var customer = await Task.Run(() => new Customer("Thanuja", "thanu@gmail.com"));
            var ticket = await Task.Run(() => new Ticket("Sample Ticket", "Description here", customer)
            {
                Priority = Priority.High
            });

            Assert.AreEqual(Priority.High, ticket.Priority);
        }

        // Test parsing valid string inputs into Priority enum
        [TestCase("Critical", Priority.Critical)]
        [TestCase("High", Priority.High)]
        [TestCase("Medium", Priority.Medium)]
        [TestCase("Low", Priority.Low)]
        public async Task Parse_Valid_String_To_Priority_Async(string input, Priority expected)
        {
            await Task.Run(() =>
            {
                Priority priority = Enum.Parse<Priority>(input);
                Assert.AreEqual(expected, priority);
            });
        }

        // Test that invalid string inputs return false for TryParse
        [TestCase("Urgent")]
        [TestCase("SuperHigh")]
        [TestCase("")]
        [TestCase(null)]
        public async Task TryParse_Invalid_String_Returns_False_Async(string input)
        {
            await Task.Run(() =>
            {
                bool success = Enum.TryParse(input, out Priority priority);
                Assert.IsFalse(success);
            });
        }

        // Default priority for new tickets should be Medium
        [Test]
        public async Task Default_Priority_Is_Medium_Async()
        {
            var customer = await Task.Run(() => new Customer("User", "user@gmail.com"));
            var ticket = await Task.Run(() => new Ticket("Title", "Desc", customer));

            Assert.AreEqual(Priority.Medium, ticket.Priority);
        }

        // Changing the Priority property should update correctly
        [TestCase(Priority.Low)]
        [TestCase(Priority.Medium)]
        [TestCase(Priority.High)]
        [TestCase(Priority.Critical)]
        public async Task Change_Priority_Updates_Property_Async(Priority newPriority)
        {
            var customer = await Task.Run(() => new Customer("User", "user@gmail.com"));
            var ticket = await Task.Run(() => new Ticket("Title", "Desc", customer));

            ticket.Priority = newPriority;

            Assert.AreEqual(newPriority, ticket.Priority);
        }

        // Casting an invalid enum value should throw an exception
        [TestCase(999)]
        [TestCase(-1)]
        [TestCase(1000)]
        public async Task Invalid_Enum_Cast_Throws_Exception_Async(int invalidValue)
        {
            await Task.Run(() =>
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    var invalid = (Priority)invalidValue;
                    if (!Enum.IsDefined(typeof(Priority), invalid))
                        throw new ArgumentException("Invalid Priority enum value");
                });
            });
        }

        // ToString() returns correct string representation of Priority enum
        [TestCase(Priority.Low, "Low")]
        [TestCase(Priority.Medium, "Medium")]
        [TestCase(Priority.High, "High")]
        [TestCase(Priority.Critical, "Critical")]
        public async Task Priority_ToString_Returns_Correct_String_Async(Priority priority, string expectedString)
        {
            await Task.Run(() =>
            {
                string str = priority.ToString();
                Assert.AreEqual(expectedString, str);
            });
        }
    }
}
