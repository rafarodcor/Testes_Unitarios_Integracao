using System.Collections;
using System.Text;

namespace JornadaMilhasV1.Validador;

internal interface IValidavel
{
    bool EhValido { get; }
    Erros Erros { get; }
}

public class Erros : IEnumerable<Erro>
{
    private readonly ICollection<Erro> erros = new List<Erro>();

    public void RegistrarErro(string mensagem) => erros.Add(new Erro(mensagem));

    public IEnumerator<Erro> GetEnumerator() => erros.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => erros.GetEnumerator();

    public string Sumario
    {
        get
        {
            var sb = new StringBuilder();
            foreach (var item in erros)
                sb.AppendLine(item.Mensagem);
            return sb.ToString();
        }
    }
}

public record Erro(string Mensagem);