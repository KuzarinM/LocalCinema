using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdstractHelpers.Mediator.Models;

namespace AdstractHelpers.Mediator.Exceptions
{
    [Serializable]
    public class MediatorInnerException : Exception
    {
        public ResponseModel? ResponseModel { get; set; }

        public MediatorInnerException(ResponseModel? model = null) : base()
        {
            ResponseModel = model;
        }
        public MediatorInnerException(string message, ResponseModel? model = null) : base(message)
        {
            ResponseModel = model;
        }
        public MediatorInnerException(string message, Exception inner, ResponseModel? model = null) : base(message, inner)
        {
            ResponseModel = model;
        }
    }
}
