using AdstractHelpers.Mediator.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdstractHelpers.Mediator.Interfaces
{
    public interface IRequestModel<T> : IRequest<ResponseModel<T>>
    {
    }

    public interface IRequestModel : IRequest<ResponseModel>
    {
    }
}
