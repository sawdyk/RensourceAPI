using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Response
{
    public class GenericResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        public object? Data { get; set; }
    }

    public class PaginationResponse : GenericResponse
    {
        public long TotalData { get; set; }
    }
}
