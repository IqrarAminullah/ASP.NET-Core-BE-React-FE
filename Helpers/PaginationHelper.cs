using TestApi.Filters;
using TestApi.Services;
using TestApi.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter,int totalRecords, IUriService uriService, string route)
        {
            var response = new PagedResponse<List<T>>(pagedData, validFilter.pageNumber, validFilter.pageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.pageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages)); 
            response.nextPage =
                validFilter.pageNumber >= 1 && validFilter.pageNumber < roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.pageNumber + 1, validFilter.pageSize), route)
                : null;
            response.previousPage =
                validFilter.pageNumber - 1 >= 1 && validFilter.pageNumber <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.pageNumber - 1, validFilter.pageSize), route)
                : null;
            response.firstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.pageSize), route);
            response.lastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.pageSize), route);
            response.totalPages = roundedTotalPages;
            response.totalRecords = totalRecords;
            return response;
        }
    }
}