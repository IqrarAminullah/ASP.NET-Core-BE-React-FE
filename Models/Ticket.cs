using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace TestApi.Models
{
    public enum TicketStatus{
        None,
        Open,
        InProgress,
        Done,
        Closed
    }

    public class Ticket
    {
        public int ticketId {get;set;}
        public string ticketName {get;set;}
        public string description {get;set;}
        public DateTime dateCreated {get;set;}
        public int? assigneeId {get;set;}
        public int? ownerId {get;set;}
        public int categoryId {get;set;}
        public int statusId {get;set;}

        [NotMapped]
        //[JsonConverter(typeof(StringEnumConverter))]
        public TicketStatus status
        {
            get
            {
                return (TicketStatus)this.statusId;
            }
            set
            {
                this.statusId = (int)value;
            }
        }

        public virtual User assignee {get;set;}
        public virtual User owner {get;set;}
        public virtual Category category {get;set;}
    }
}