namespace Discount.gRPC.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<CouponModel> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon == null) 
            {
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            }

            logger.LogInformation("Discount is retrieved for Id: {id}, ProductName: {productName}, Amount: {amount}, Description: {description}", coupon.Id, coupon.ProductName, coupon.Amount, coupon.Description);
            var couponRPCModel = coupon.Adapt<CouponModel>();
            return couponRPCModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
            }
            
            await dbContext.Coupons.AddAsync(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully created. ProductName: {productName}, Amount: {amount}, Description: {description}", coupon.ProductName, coupon.Amount, coupon.Description);

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
            }

            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully updated. ProductName: {productName}, Amount: {amount}, Description: {description}", coupon.ProductName, coupon.Amount, coupon.Description);
            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.Where(x => x.ProductName.Equals(request.ProductName)).FirstAsync();   

            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Discount with ProductName: {request.ProductName} does not exist."));
            }

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully delete. Id: {id}, ProductName: {productName}, Amount: {amount}, Description: {description}",coupon.Id, coupon.ProductName, coupon.Amount, coupon.Description);
            return new DeleteDiscountResponse { Success = true };
        }
    }
}
