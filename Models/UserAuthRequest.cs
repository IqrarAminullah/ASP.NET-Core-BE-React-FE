using System.ComponentModel.DataAnnotations;

namespace TestApi.Models
{
    public class UserAuthRequest
    {
        public string password{get;set;}
        public string email {get;set;}
    }
}