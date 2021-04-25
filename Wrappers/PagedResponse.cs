using System;

namespace TestApi.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int pageNumber {get;set;}
        public int pageSize {get;set;}
        public Uri firstPage {get;set;}
        public Uri lastPage {get;set;}
        public int totalPages {get;set;}
        public int totalRecords {get;set;}
        public Uri nextPage {get;set;}
        public Uri previousPage {get;set;}

        public PagedResponse(T responseData, int pageNumber, int pageSize)
        {
            this.data = responseData;
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            this.message = null;
            this.succeeded = true;
            this.errors = null;
        }

    }
}