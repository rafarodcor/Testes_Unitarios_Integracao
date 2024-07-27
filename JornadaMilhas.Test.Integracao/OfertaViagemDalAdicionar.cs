using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao;

[Collection(nameof(ContextCollection))]
public class OfertaViagemDalAdicionar
{
    private readonly JornadaMilhasContext _context;

    public OfertaViagemDalAdicionar(ITestOutputHelper output, ContextFixture fixture)
    {
        _context = fixture.Context;
        output.WriteLine(_context.ContextId.ToString());
    }

    [Fact]
    public void RegistraOfertaNoBanco()
    {
        //arrange
        var oferta = CriarOfertaViagem();
        var dal = new OfertaViagemDAL(_context);

        //act
        dal.Adicionar(oferta);

        //assert
        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);
        Assert.NotNull(ofertaIncluida);
        Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.0001);
    }

    [Fact]
    public void RegistraOfertaNoBancoComInformacoesCorretas()
    {
        //arrange
        var oferta = CriarOfertaViagem();
        var dal = new OfertaViagemDAL(_context);

        //act
        dal.Adicionar(oferta);

        //assert
        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);
        Assert.Equal(ofertaIncluida.Rota.Origem, oferta.Rota.Origem);
        Assert.Equal(ofertaIncluida.Rota.Destino, oferta.Rota.Destino);
        Assert.Equal(ofertaIncluida.Periodo.DataInicial, oferta.Periodo.DataInicial);
        Assert.Equal(ofertaIncluida.Periodo.DataFinal, oferta.Periodo.DataFinal);
        Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
    }

    private OfertaViagem CriarOfertaViagem()
    {
        Rota rota = new Rota("São Paulo", "Fortaleza");
        Periodo periodo = new Periodo(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
        double preco = 350;

        return new OfertaViagem(rota, periodo, preco);
    }
}