using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ToDoApplication
{
    /// <summary>
    /// Application entry point class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Application entry point function.
        /// </summary>
        /// <param name="args">Arguments for application.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates Host builder.
        /// </summary>
        /// <param name="args">Arguments from CMD.</param>
        /// <returns>Program abstraction.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
