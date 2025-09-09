using Bogus;
using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Tests.BusinessTests.Models.Fixtures
{
    [CollectionDefinition(nameof(EnderecoFixtureCollection))]
    public class EnderecoFixtureCollection : ICollectionFixture<EnderecoFixture>
    {
    }
    public class EnderecoFixture : IDisposable
    {

        public Endereco GerarEnderecoValido()
        {
            var endereco = new Faker<Endereco>(locale: "pt_BR")
                .CustomInstantiator(f => new Endereco
                {
                    Logradouro = f.Address.StreetName(),
                    Numero = f.Random.Number(1, 1000).ToString(),
                    Complemento = f.Address.SecondaryAddress(),
                    Bairro = f.Address.StreetName(), // Usando StreetName para gerar um nome de bairro
                    Cep = f.Address.ZipCode("########"),
                    Cidade = f.Address.City(),
                    Estado = f.Address.StateAbbr()
                });

            return endereco;
        }


        public void Dispose()
        {
        }
    }
}
