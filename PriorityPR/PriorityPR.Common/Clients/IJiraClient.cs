using PriorityPR.Common.Models;

namespace PriorityPR.Common.Clients;

public interface IJiraClient
{
    Task<Dictionary<string, Ticket>> GetCurrentSprintTickets();
}