﻿using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhasV1.Gerenciador;
public class GerenciadorDeOfertas
{
    private List<OfertaViagem> _ofertaViagem = new List<OfertaViagem>();
    OfertaViagemDAL ofertaViagemDAL = new OfertaViagemDAL(new JornadaMilhasContext());

    public GerenciadorDeOfertas(List<OfertaViagem> ofertaViagem) => _ofertaViagem = ofertaViagem;

    public void CadastrarOferta()
    {
        Console.WriteLine("-- Cadastro de ofertas --");
        Console.WriteLine("Informe a cidade de origem: ");
        string origem = Console.ReadLine();

        Console.WriteLine("Informe a cidade de destino: ");
        string destino = Console.ReadLine();

        Console.WriteLine("Informe a data de ida (DD/MM/AAAA): ");
        DateTime dataIda;
        if (!DateTime.TryParse(Console.ReadLine(), out dataIda))
        {
            Console.WriteLine("Data de ida inválida.");
            return;
        }

        Console.WriteLine("Informe a data de volta (DD/MM/AAAA): ");
        DateTime dataVolta;
        if (!DateTime.TryParse(Console.ReadLine(), out dataVolta))
        {
            Console.WriteLine("Data de volta inválida.");
            return;
        }

        Console.WriteLine("Informe o preço: ");
        double preco;
        if (!double.TryParse(Console.ReadLine(), out preco))
        {
            Console.WriteLine("Formato de preço inválido.");
            return;
        }

        OfertaViagem ofertaCadastrada = new OfertaViagem(new Rota(origem, destino), new Periodo(dataIda, dataVolta), preco);
        ofertaViagemDAL.Adicionar(ofertaCadastrada);

        Console.WriteLine("\nOferta cadastrada com sucesso.");
    }

    public bool AdicionarOfertaNaLista(OfertaViagem ofertaCadastrada)
    {
        if (ofertaCadastrada != null)
        {
            _ofertaViagem.Add(ofertaCadastrada);
            return true;
        }
        return false;
    }

    public OfertaViagem? RecuperaMaiorDesconto(Func<OfertaViagem, bool> filtro) => _ofertaViagem
        .Where(filtro)
        .Where(o => o.Ativa)
        .OrderBy(o => o.Preco)
        .FirstOrDefault();

    public void CarregarOfertas()
    {
        AdicionarOfertaNaLista(new OfertaViagem(new Rota("São Paulo", "Curitiba"), new Periodo(new DateTime(2024, 1, 15), new DateTime(2024, 1, 20)), 500));
        AdicionarOfertaNaLista(new OfertaViagem(new Rota("Recife", "Rio de Janeiro"), new Periodo(new DateTime(2024, 2, 10), new DateTime(2024, 2, 15)), 700));
        AdicionarOfertaNaLista(new OfertaViagem(new Rota("Acre", "Brasília"), new Periodo(new DateTime(2024, 3, 5), new DateTime(2024, 3, 10)), 600));
    }

    public void ExibirTodasAsOfertas()
    {
        Console.WriteLine("\nTodas as ofertas cadastradas: ");
        foreach (var oferta in _ofertaViagem)
        {
            Console.WriteLine(oferta);
        }
    }
}