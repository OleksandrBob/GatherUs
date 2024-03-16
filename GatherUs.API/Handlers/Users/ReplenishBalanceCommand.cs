using CSharpFunctionalExtensions;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Users;

public class ReplenishBalanceCommand : IRequest<Result<decimal, FormattedError>>
{
    internal int UserId { get; set; }
    
    public string Nonce { get; set; }

    public decimal Amount { get; set; }

    public class Handler : IRequestHandler<ReplenishBalanceCommand, Result<decimal, FormattedError>>
    {
        private readonly IPaymentService _paymentService;

        public Handler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<Result<decimal, FormattedError>> Handle(ReplenishBalanceCommand request,
            CancellationToken ct)
        {
            var result = await _paymentService.ReplenishBalance(request.UserId, request.Nonce, request.Amount);

            if (result.IsFailure)
            {
                return Result.Failure<decimal, FormattedError>(new(result.Error));
            }

            return request.Amount;
        }
    }
}
