using AdstractHelpers.Extentions;
using AdstractHelpers.Mediator.Exceptions;
using AdstractHelpers.Mediator.Interfaces;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdstractHelpers.Mediator.Abstractions
{
    /// <summary>
    /// Абстрактная обёртка над медиатором
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public abstract class AbstractCommandHandler<TSource> : IRequestHandler<TSource, ResponseModel>
        where TSource : IRequestModel
    {
        protected readonly ILogger _logger;
        protected readonly IMediator? _mediator;

        public AbstractCommandHandler(ILogger logger, IMediator? mediator = null)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ResponseModel> Handle(TSource request, CancellationToken cancellationToken)
        {
            try
            {
                request.ValidateObjectAndThrow();

                _logger.LogDebug("Start {handler}", GetType().Name);

                return await HandleAsync(request, cancellationToken);
            }
            catch (MediatorInnerException ex)
            {
                return Error(ex.Message, ex, ex.ResponseModel?.StatusCode ?? 400);
            }
            catch (Exception ex)
            {
                return Error("Unexpected Error", ex);
            }
        }

        public async Task<TTo> MediatorSendAsync<TTo>(IRequest<ResponseModel<TTo>> request)
        {
            if (_mediator == null)
                throw new NotImplementedException("Mediator not set!");

            var res = await _mediator.Send(request);

            if (!res.IsError)
                return res.Data;

            if (res.Exception != null)
                throw new MediatorInnerException(res.ErrorMessage!, res.Exception, res);
            else
                throw new MediatorInnerException(res.ErrorMessage!, res);
        }

        public abstract Task<ResponseModel> HandleAsync(TSource request, CancellationToken cancellationToken);

        protected ResponseModel Success(int code = 200)
        {
            _logger.LogDebug("[Success] end {handler}", GetType().Name);

            return new()
            {
                StatusCode = code,
                ErrorMessage = null
            };
        }

        protected Task<ResponseModel> SuccessTask(int code = 200) => Task.FromResult(Success(code));

        protected ResponseModel Error(string message = "Во время выполнения произошла ошибка", Exception? error = null, int code = 400)
        {
            if (error != null)
                _logger.LogError(message, error);

            _logger.LogDebug("[Fail] end {handler}", GetType().Name);

            return new()
            {
                StatusCode = code,
                ErrorMessage = message
            };
        }

        protected Task<ResponseModel> ErrorTask(string message = "Во время выполнения произошла ошибка", Exception? error = null, int code = 400) => Task.FromResult(Error(message, error, code));
    }
}
