using System;
using TestApi.Models;

namespace TestApi.Filters
{
    public class TicketFilter
    {
        public TicketStatus ticketType {get;set;}

        public TicketFilter()
        {
            ticketType = new TicketStatus();
        }

        public TicketFilter(TicketStatus type)
        {
            ticketType = type;
        }
    }
}