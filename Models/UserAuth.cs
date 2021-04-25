using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApi.Models
{
    public class UserAuth
    {
        public int userAuthId {get;set;}
        public int userId {get;set;}
        public string password {get;set;}

        public virtual User user {get;set;}
    }
}