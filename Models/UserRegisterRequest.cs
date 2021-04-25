using System.ComponentModel.DataAnnotations;

namespace TestApi.Models
{
    public class UserRegisterRequest
    {
        public string username{get;set;}
        public string password{get;set;}
        public string email {get;set;}
    }
}