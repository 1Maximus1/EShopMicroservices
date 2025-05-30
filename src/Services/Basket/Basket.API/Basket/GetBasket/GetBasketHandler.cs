﻿using Basket.API.Data;

namespace Basket.API.Basket.GetBusket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart Cart);

    public class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var cart = await repository.GetBasket(query.UserName);
            return new GetBasketResult(cart);
        }
    }
}
