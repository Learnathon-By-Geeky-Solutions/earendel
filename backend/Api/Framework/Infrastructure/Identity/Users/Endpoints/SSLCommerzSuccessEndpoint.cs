using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TalentMesh.Framework.Infrastructure.Messaging;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints
{
    [ExcludeFromCodeCoverage]
    public static class SslCommerzSuccessEndpoint
    {
        internal static RouteHandlerBuilder MapSslCommerzSuccessEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapPost("/success", async (
                [FromServices] ILogger<object> logger,
                [FromServices] IExternalApiClient externalApiClient,
                [FromServices] IMessageBus _messageBus,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                // Ensure the request has a proper content type.
                if (!context.Request.HasFormContentType)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid request: form content required.", cancellationToken);
                    return "Invalid";
                }

                // Read the form values from the POST request.
                var form = await context.Request.ReadFormAsync(cancellationToken);

                // Extract the val_id from the form. This is the key field used to validate the payment.
                var valId = form["val_id"].ToString();
                if (string.IsNullOrWhiteSpace(valId))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Parameter 'val_id' is missing.", cancellationToken);
                    return "Invalid";
                }

                try
                {
                    // Extract additional fields for logging and process tracking.
                    var tranId = form["tran_id"].ToString();
                    var status = form["status"].ToString();
                    var amount = form["amount"].ToString();
                    var cardType = form["card_type"].ToString();

                    // Call the validation API using the extracted val_id.
                    var response = await externalApiClient.ValidateSslCommerzPaymentAsync(valId);

                    if (string.Equals(response, "VALID", StringComparison.OrdinalIgnoreCase))
                    {                        
                        var publishMessage = new
                        {
                            JobId = tranId,
                            Status = "Success",
                            RequestedBy = "2b982b04-6d0e-4133-93ac-7dddff87c7f5"
                        };

                        await _messageBus.PublishAsync(publishMessage, "job.payment.update.events", "job.payment.update.fetched", cancellationToken);

                        return $"Payment validated successfully with val_id: {valId}";
                    }
                    else
                    {
                        return $"Payment validation failed. Response status: {response}";
                    }
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "Failed to process SSLCommerz success callback");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Payment validation failed due to an internal error.", cancellationToken);
                    return "Invalid";
                }
            })
            .AllowAnonymous()
            .WithName("SslCommerzSuccess")
            .WithDisplayName("SSLCommerz Success Endpoint");
        }
    }
}
