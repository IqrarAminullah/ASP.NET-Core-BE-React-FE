namespace TestApi.Models
{
    public class UserAuthResponse
    {
        public int id{get;set;}
        public string username{get;set;}
        public string token {get;set;}

        public UserAuthResponse(User user, string token)
        {
            this.id = user.userId;
            this.username = user.username;
            this.token = token;
        }
    }
}