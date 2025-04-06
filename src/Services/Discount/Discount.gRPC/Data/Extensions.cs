namespace Discount.gRPC.Data
{
    public static class Extensions
    {
        public static IApplicationBuilder UseMigrations(this IApplicationBuilder app) 
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<DiscountContext>();
            dbContext.Database.Migrate();
            
            return app;
        }
    }
}
