using System;
namespace TestApi.Models
{

    public class TicketRequest
    {
        
        public string ticketName {get;set;}
        public string description {get;set;}
        public int? assigneeId {get;set;}
        public int? ownerId {get;set;}
        public int categoryId {get;set;}

        public TicketRequest(string name, string desc, int? aId, int? oId, int cId){
            this.ticketName = name;
            this.description =desc;
            this.assigneeId = aId;
            this.ownerId = oId;
            this.categoryId = cId;
        }
    }
}