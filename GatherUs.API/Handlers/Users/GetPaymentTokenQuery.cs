using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Users;

public class GetPaymentTokenQuery : IRequest<Result<string>>
{
    internal int UserId { get; init; }

    public class Handler : IRequestHandler<GetPaymentTokenQuery, Result<string>>
    {
        private readonly IPaymentService _paymentService;

        public Handler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<Result<string>> Handle(GetPaymentTokenQuery request, CancellationToken cancellationToken)
        {
            return await _paymentService.GenerateClientToken(request.UserId);
        }
    }
}
