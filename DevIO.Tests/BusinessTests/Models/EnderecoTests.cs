using DevIO.Business.Models;
using DevIO.Business.Models.Validations;
using DevIO.Tests.BusinessTests.Models.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Tests.BusinessTests.Models
{
    [Collection(nameof(EnderecoFixtureCollection))]
    public class EnderecoTests
    {
        readonly EnderecoFixture _enderecoFixture;
        public EnderecoTests(EnderecoFixture enderecoFixture)
        {
            _enderecoFixture = enderecoFixture;
        }

        [Fact]
        public void Endereco_NovoEndereco_EhValido()
        {
            // Arrange
            var endereco = _enderecoFixture.GerarEnderecoValido();
            // Act
            var result = new EnderecoValidation().Validate(endereco);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Endereco_NovoEndereco_EhInvalido()
        {
            // Arrange
            var endereco = new Endereco
            {
                Logradouro = "",
                Numero = "",
                Complemento = "Apto 1",
                Bairro = "",
                Cep = "1234",
                Cidade = "",
                Estado = ""
            };
            // Act
            var result = new EnderecoValidation().Validate(endereco);
            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }
    }
}
