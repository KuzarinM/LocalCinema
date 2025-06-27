using AdstractHelpers.Mediator.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdstractHelpers.Controller
{
    public class MediatorContoller(IMediator mediator): ControllerBase
    {

        protected IMediator _mediator = mediator;

        protected async Task<IActionResult> MediatorSendRequest<T>(
            IRequestModel<T> request,
            CancellationToken cancellationToken = default,
            string? onErrorText = "Произошла ошибка")
        {
            try
            {
                var res = await mediator.Send(request, cancellationToken);

                if (!res.IsError)
                {
                    return new ObjectResult(res.Data)
                    {
                        StatusCode = res.StatusCode,
                    };
                }

                return new ObjectResult(onErrorText ?? res.ErrorMessage)
                {
                    StatusCode = res.StatusCode
                };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.ToString())
                {
                    StatusCode = 500
                };
            }
        }

        protected async Task<IActionResult> MediatorSendRequest(
            IRequestModel request,
            CancellationToken cancellationToken = default,
            string? onErrorText = "Произошла ошибка")
        {
            try
            {
                var res = await mediator.Send(request, cancellationToken);

                if (!res.IsError)
                {
                    return new StatusCodeResult(res.StatusCode);
                }

                return new ObjectResult(onErrorText ?? res.ErrorMessage)
                {
                    StatusCode = res.StatusCode
                };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.ToString())
                {
                    StatusCode = 500
                };
            }
        }
    }
}
