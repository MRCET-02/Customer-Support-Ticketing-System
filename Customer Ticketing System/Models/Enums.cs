using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Ticketing_System.Models
{
    public enum TicketStatus { Open, InProgress, Resolved, Closed, Escalated }
    public enum PriorityLevel { Low, Medium, High, Urgent }
}
