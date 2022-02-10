using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System; 
using System.Linq; 
using System.Threading.Tasks;
using TradeChat.Models.Options;
using TradeChat.Models.ViewModels;

namespace TradeChat.Services.Ecrypt
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EncryptedStreamAttribute : Attribute , IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowUncryptedStreamAttribute));
            if (context == null || hasAllowAnonymous)
            {
                await next();
                return;
            }

            try
            {
                using (var scope = context.HttpContext.RequestServices.CreateScope())
                {
                    IServiceProvider scopedServices = scope.ServiceProvider;
                    IOptions<EncryptionOptions> options = scopedServices.GetRequiredService<IOptions<EncryptionOptions>>();
                    if(context.ActionArguments.Count() > 0)
                    {
                        var requestBody = context.ActionArguments["request"] as ApikeyRequest;

                        if (!string.IsNullOrEmpty(requestBody.ApiKey))
                        {
                            requestBody.ApiKey = Encryption.DecryptText(requestBody.ApiKey, options.Value.BrokerAccountKey);
                        }

                        if (!string.IsNullOrEmpty(requestBody.ApiSecret))
                        {
                            requestBody.ApiSecret = Encryption.DecryptText(requestBody.ApiSecret, options.Value.BrokerAccountKey);
                        }

                        if (!string.IsNullOrEmpty(requestBody.Code))
                        {
                            requestBody.Code = Encryption.DecryptText(requestBody.Code, options.Value.BrokerAccountKey);
                        }

                        if (!string.IsNullOrEmpty(requestBody.State))
                        {
                            requestBody.State = Encryption.DecryptText(requestBody.State, options.Value.BrokerAccountKey);
                        }
                        if (requestBody.Otp != 0)
                        {
                            requestBody.Otp = Convert.ToInt64(Encryption.DecryptText(requestBody.Otp.ToString(), options.Value.BrokerAccountKey));
                        }

                        context.ActionArguments["request"] = requestBody;
                        await next();
                        return;
                    }
                    await next();
                    return;
                }
            }
            catch (Exception x)
            {

                throw x;
            }

           

        }


    }

    
}
