using Microsoft.AspNetCore.Builder;

namespace TradeChat.API.Extensions
{
    public static class AppBuilderCollectionExtensions
    {
        public static void UseSwaggerServices(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trade Chat API - v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
