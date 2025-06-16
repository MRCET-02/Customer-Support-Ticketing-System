using Customer_Ticketing_System.Models;
using System.Text.Json;

namespace Customer_Ticketing_System.Services
{
    public static class TicketFileHandler
    {
        private static readonly string filePath = "tickets.json";

        public static async Task<List<Ticket>> ReadTicketsAsync()
        {
            if (!File.Exists(filePath))
                return new List<Ticket>();

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Ticket>>(json) ?? new List<Ticket>();
        }

        public static async Task WriteTicketsAsync(List<Ticket> tickets)
        {
            var json = JsonSerializer.Serialize(tickets, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}

