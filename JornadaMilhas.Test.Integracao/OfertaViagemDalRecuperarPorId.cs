using JornadaMilhas.Dados;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao
{
    [Collection(nameof(ContextCollection))]
    public class OfertaViagemDalRecuperarPorId
    {
        private readonly JornadaMilhasContext _context;

        public OfertaViagemDalRecuperarPorId(ITestOutputHelper output, ContextFixture fixture)
        {
            _context = fixture.Context;
            output.WriteLine(_context.ContextId.ToString());
        }

        [Fact]
        public void RestornaNuloQuandoIdInexistente()
        {
            //arrange
            var dal = new OfertaViagemDAL(_context);

            //act
            var ofertaRecuperada = dal.RecuperarPorId(-2);

            //assert
            Assert.Null(ofertaRecuperada);
        }
    }
}