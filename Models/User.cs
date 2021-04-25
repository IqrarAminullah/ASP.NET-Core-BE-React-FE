using System;
using System.Collections.Generic;

namespace TestApi.Models
{
    public class User
    {
        public int userId {get;set;}
        public string username {get;set;}
        public string email {get;set;}

        public virtual ICollection<Ticket> ownedTickets {get;set;}
        public virtual ICollection<Ticket> assignedTickets {get;set;}
        public virtual UserAuth auth {get;set;}
    }
}