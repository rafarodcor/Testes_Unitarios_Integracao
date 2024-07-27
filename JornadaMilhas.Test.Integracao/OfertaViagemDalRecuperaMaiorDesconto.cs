using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhasV1.Gerenciador;
using JornadaMilhasV1.Modelos;
using Org.BouncyCastle.Crypto.Digests;

namespace JornadaMilhas.Test.Integracao
{
    [Collection((nameof(ContextCollection)))]
    public class OfertaViagemDalRecuperaMaiorDesconto : IDisposable
    {
        private readonly JornadaMilhasContext _context;
        private readonly ContextFixture _fixture;

        public OfertaViagemDalRecuperaMaiorDesconto(ContextFixture fixture)
        {
            _context = fixture.Context;
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.LimpaDadosDoBanco();
        }

        [Fact]
        // destino = são paulo, desconto = 40, preco = 80
        public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
        {
            //arrange
            var rota = new RotaDataBuilder() { Origem = "Curitiba", Destino = "São Paulo" }.Build();
            Periodo periodo = new PeriodoDataBuilder() { DataInicial = new DateTime(2024, 5, 20) }.Build();
            _fixture.CriaDadosFake();

            var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
            {
                Desconto = 40,
                Ativa = true
            };

            var dal = new OfertaViagemDAL(_context);
            dal.Adicionar(ofertaEscolhida);

            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
            var precoEsperado = 40;

            //act
            var oferta = dal.RecuperaMaiorDesconto(filtro);

            //assert
            Assert.NotNull(oferta);
            Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
        }


        [Fact]
        // destino = são paulo, desconto = 60, preco = 80
        public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto60()
        {
            //arrange
            var rota = new RotaDataBuilder() { Origem = "Curitiba", Destino = "São Paulo" }.Build();
            Periodo periodo = new PeriodoDataBuilder() { DataInicial = new DateTime(2024, 5, 20) }.Build();
            _fixture.CriaDadosFake();

            var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
            {
                Desconto = 60,
                Ativa = true
            };

            var dal = new OfertaViagemDAL(_context);
            dal.Adicionar(ofertaEscolhida);

            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
            var precoEsperado = 20;

            //act
            var oferta = dal.RecuperaMaiorDesconto(filtro);

            //assert
            Assert.NotNull(oferta);
            Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
        }
    }
}