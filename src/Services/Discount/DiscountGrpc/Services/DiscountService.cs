using Discount.Grpc.Data;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext
           .Coupons
           .FirstOrDefaultAsync(_ => _.ProductName == request.ProductName);

            if (coupon is null)
                coupon = new Models.Coupon { ProductName = "No Discount", Description = "no discount", Amount = 0 };
            logger.LogInformation("discount is retrieved");

            // Mapster here is installed not from building blocks because we need this nod depend on other thing because we need performance
            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Models.Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "invalid request object"));

            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("discount created");
            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Models.Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "invalid request object"));

            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("discount updated");
            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> Delete(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext
             .Coupons
             .FirstOrDefaultAsync(_ => _.ProductName == request.ProductName);

            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "invalid request object"));

            logger.LogInformation("discount is retrieved");


            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("discount saves");
            return new DeleteDiscountResponse { Success = true };
        }
    }
}
