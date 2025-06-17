using Customer_Ticketing_System.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TicketManagementSystem;

namespace Customer_Ticketing_System.Services
{
    public static class TicketFileHandler
    {
        private static readonly string filePath;

        static TicketFileHandler()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(baseDirectory, "Data");
            Directory.CreateDirectory(dataDirectory);
            filePath = Path.Combine(dataDirectory, "tickets.json");
        }

        public static async Task<List<Ticket>> ReadTicketsAsync()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<Ticket>();
                }

                var json = await File.ReadAllTextAsync(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Ticket>();
                }

                return JsonSerializer.Deserialize<List<Ticket>>(json) ?? new List<Ticket>();
            }
            catch (JsonException ex)
            {
                await Logger.LogExceptionAsync(ex);
                throw;
            }
            catch (IOException ex)
            {
                await Logger.LogExceptionAsync(ex);
                throw;
            }
        }

        public static async Task WriteTicketsAsync(List<Ticket> tickets)
        {
            try
            {
                var json = JsonSerializer.Serialize(tickets, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (IOException ex)
            {
                await Logger.LogExceptionAsync(ex);
                throw;
            }
        }
    }
}