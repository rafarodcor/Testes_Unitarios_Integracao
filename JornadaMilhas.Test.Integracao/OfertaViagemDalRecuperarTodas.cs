using JornadaMilhas.Dados;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao
{
    [Collection((nameof(ContextCollection)))]
    public class OfertaViagemDalRecuperarTodas
    {
        private readonly JornadaMilhasContext _context;

        public OfertaViagemDalRecuperarTodas(ITestOutputHelper output, ContextFixture fixture)
        {
            _context = fixture.Context;
            output.WriteLine(_context.ContextId.ToString());
        }

        [Fact]
        public void RestornaQuandoRetornoNaoNulo()
        {
            //arrange
            var dal = new OfertaViagemDAL(_context);

            //act
            var ofertasRecuperada = dal.RecuperarTodas();

            //assert
            Assert.NotNull(ofertasRecuperada);
        }
    }
}