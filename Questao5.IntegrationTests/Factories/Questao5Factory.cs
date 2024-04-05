using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Questao5.Infrastructure.Sqlite;
using Questao5.IntegrationTests.Fixtures;

namespace Questao5.IntegrationTests.Factories
{
    [Collection("Database")]
    public class Questao5Factory : WebApplicationFactory<Program>
    {
        private readonly DbFixture _dbfixture;

        public Questao5Factory(DbFixture dbFixture)
        {
            _dbfixture = dbFixture;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            //Faz a troca da Singleton que controla a configuração da database para a database de testes.
            builder.ConfigureServices(services =>
            {
               services.AddSingleton(new DatabaseConfig { Name = $"Data Source = { _dbfixture.DatabaseName }" });
            });
        }
    }
}
