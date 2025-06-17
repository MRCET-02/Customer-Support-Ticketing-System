using Customer_Ticketing_System.Models;

namespace Customer_Ticketing_System.Tests
{
    [TestFixture]
    public class TimeRangeTests
    {
        private List<Ticket> tickets = new();

        [SetUp]
        public async Task Setup()
        {
            var customer = await Task.Run(() => new Customer("Thanu", "thanu@gmail.com"));

            tickets = await Task.Run(() => new List<Ticket>
            {
                new Ticket("Ticket1", "Desc1", customer) { CreatedAt = DateTime.Now.AddDays(-5) },
                new Ticket("Ticket2", "Desc2", customer) { CreatedAt = DateTime.Now.AddDays(-3) },
                new Ticket("Ticket3", "Desc3", customer) { CreatedAt = DateTime.Now.AddDays(-1) },
                new Ticket("Ticket4", "Desc4", customer) { CreatedAt = DateTime.Now }
            });
        }


        [TestCase(-4, 0, 3)]
        [TestCase(-5, -3, 2)]
        [TestCase(-2, 0, 2)]
        [TestCase(-6, -1, 3)]
        [TestCase(-1, 0, 2)]
        public async Task Filter_Tickets_Within_TimeRange_Async(int startDaysAgo, int endDaysAgo, int expectedCount)
        {
            DateTime start = DateTime.Now.AddDays(startDaysAgo).Date;
            DateTime end = DateTime.Now.AddDays(endDaysAgo).Date;

            var filteredTickets = await Task.Run(() =>
                tickets.Where(t => t.CreatedAt.Date >= start && t.CreatedAt.Date <= end).ToList()
            );

            Assert.AreEqual(expectedCount, filteredTickets.Count);
            Assert.IsTrue(filteredTickets.All(t => t.CreatedAt.Date >= start && t.CreatedAt.Date <= end));
        }

        [TestCase(-10, -6)]
        [TestCase(-20, -15)]
        [TestCase(-12, -8)]
        [TestCase(-30, -25)]
        [TestCase(-100, -50)]
        public async Task No_Tickets_Outside_TimeRange_Async(int startDaysAgo, int endDaysAgo)
        {
            DateTime start = DateTime.Now.AddDays(startDaysAgo);
            DateTime end = DateTime.Now.AddDays(endDaysAgo);

            var filteredTickets = await Task.Run(() =>
                tickets.Where(t => t.CreatedAt >= start && t.CreatedAt <= end).ToList()
            );

            Assert.AreEqual(0, filteredTickets.Count);
        }

        [TestCase(-5, -3, 2)]
        [TestCase(-4, -3, 1)]
        [TestCase(-5, -4, 1)]
        [TestCase(-3, -1, 2)]
        [TestCase(-1, 0, 2)]
        public async Task Tickets_Exactly_At_Boundary_Are_Included_Async(int startDaysAgo, int endDaysAgo, int expectedCount)
        {
            DateTime start = DateTime.Now.AddDays(startDaysAgo).Date;
            DateTime end = DateTime.Now.AddDays(endDaysAgo).Date;

            var filteredTickets = await Task.Run(() =>
                tickets.Where(t => t.CreatedAt.Date >= start && t.CreatedAt.Date <= end).ToList()
            );

            Assert.AreEqual(expectedCount, filteredTickets.Count);

            bool hasStartBoundary = filteredTickets.Any(t => t.CreatedAt.Date == start);
            bool hasEndBoundary = filteredTickets.Any(t => t.CreatedAt.Date == end);

            Assert.IsTrue(hasStartBoundary || hasEndBoundary);
        }


        [TestCase(-2, 2)]
        [TestCase(-1, 2)]
        [TestCase(-3, 1)]
        [TestCase(-4, 1)]
        [TestCase(0, 3)]
        public async Task Filter_Tickets_Created_Before_Date_Async(int cutoffDaysAgo, int expectedCount)
        {
            DateTime cutoff = DateTime.Now.AddDays(cutoffDaysAgo).Date;  

            var filteredTickets = await Task.Run(() =>
                tickets.Where(t => t.CreatedAt.Date < cutoff).ToList() 
            );

            Assert.AreEqual(expectedCount, filteredTickets.Count);
            Assert.IsTrue(filteredTickets.All(t => t.CreatedAt.Date < cutoff));
        }


        [TestCase(-2, 2)]
        [TestCase(-1, 1)]
        [TestCase(-3, 2)]
        [TestCase(-4, 3)]
        [TestCase(0, 0)]
        public async Task Filter_Tickets_Created_After_Date_Async(int cutoffDaysAgo, int expectedCount)
        {
            DateTime cutoff = DateTime.Now.AddDays(cutoffDaysAgo);

            var filteredTickets = await Task.Run(() =>
                tickets.Where(t => t.CreatedAt > cutoff).ToList()
            );

            Assert.AreEqual(expectedCount, filteredTickets.Count);
            Assert.IsTrue(filteredTickets.All(t => t.CreatedAt > cutoff));
        }
    }
}
