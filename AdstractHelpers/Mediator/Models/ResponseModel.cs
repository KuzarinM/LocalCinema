using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdstractHelpers.Mediator.Models
{
    public class ResponseModel
    {
        public string? ErrorMessage = null;

        public bool IsError => ErrorMessage != null;

        public int StatusCode { get; set; }

        public Exception? Exception { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }
    }
}
