using api.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace api.Middleware
{
    public class DatabaseReadinessMiddleware
    {
        private readonly RequestDelegate _next;

        public DatabaseReadinessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDBContext dbContext)
        {
            try
            {
                await dbContext.Database.ExecuteSqlRawAsync("SELECT 1");
            }
            catch (SqlException)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Database is not ready. Please try again later.");
                return;
            }

            await _next(context);
        }

    }
}
