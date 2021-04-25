using System;

namespace TestApi.Wrappers
{
    public class Response<T>
    {
        public T data {get;set;}
        public bool succeeded {get;set;}
        public string[] errors{get;set;}
        public string message {get;set;}

        public Response(bool success, string[] errors, string message)
        {
            this.succeeded = success;
            this.errors = errors;
            this.message = message;
            this.data = default(T);
        }

        public Response(){
            this.succeeded = true;
            this.errors = null;
            this.message = string.Empty;
            this.data = default(T);
        }
        
        public Response(T data){
            this.succeeded = true;
            this.errors = null;
            this.message = string.Empty;
            this.data = data;
        }
    }
}