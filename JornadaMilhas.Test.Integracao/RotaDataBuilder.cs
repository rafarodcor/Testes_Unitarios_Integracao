using Bogus;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test.Integracao
{
    public class RotaDataBuilder : Faker<Rota>
    {
        public string? Origem { get; set; }
        public string? Destino { get; set; }

        public RotaDataBuilder()
        {
            CustomInstantiator(f =>
            {
                string origem = Origem ?? f.Address.City();
                string destino = Destino ?? f.Address.City();
                return new Rota(origem, destino);
            });
        }

        public Rota Build() => Generate();

    }
}