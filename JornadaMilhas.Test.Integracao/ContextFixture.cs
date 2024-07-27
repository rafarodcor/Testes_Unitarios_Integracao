using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace JornadaMilhas.Test.Integracao
{
    public class ContextFixture : IAsyncLifetime
    {
        public JornadaMilhasContext Context { get; private set; }

        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .Build();

        /*public ContextFixture()
        {
            var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
              .UseSqlServer("Server=localhost,1433;Database=JornadaMilhas;User ID=sa;Password=sqlRRC00!;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")              
              .Options;

            Context = new JornadaMilhasContext(options);
        }*/

        public void CriaDadosFake()
        {
            Periodo periodo = new PeriodoDataBuilder().Build();

            var rota = new Rota("Curitiba", "São Paulo");

            var fakerOferta = new Faker<OfertaViagem>()
                .CustomInstantiator(f => new OfertaViagem(
                    rota,
                    new PeriodoDataBuilder().Build(),
                    100 * f.Random.Int(1, 100))
                )
                .RuleFor(o => o.Desconto, f => 40)
                .RuleFor(o => o.Ativa, f => true);

            var lista = fakerOferta.Generate(200);
            Context.OfertasViagem.AddRange(lista);
            Context.SaveChanges();
        }

        public async Task LimpaDadosDoBanco()
        {
            /*Context.OfertasViagem.RemoveRange(Context.OfertasViagem);
            Context.Rotas.RemoveRange(Context.Rotas);
            await Context.SaveChangesAsync();*/

            Context.Database.ExecuteSqlRaw("DELETE FROM OfertasViagem");
            Context.Database.ExecuteSqlRaw("DELETE FROM Rotas");
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
              .UseSqlServer(_msSqlContainer.GetConnectionString())
              .Options;

            Context = new JornadaMilhasContext(options);
            Context.Database.Migrate();
        }

        public async Task DisposeAsync()
        {
            await _msSqlContainer.StopAsync();
        }
    }
}