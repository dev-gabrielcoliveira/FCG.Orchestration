using FCG.Users.Tests.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Users.Tests
{
    public class UsuarioValidatorTests
    {

        private readonly UsuarioValidators _validator = new UsuarioValidators();

        #region Email

        [Fact]
        public void Email_Valido_DeveRetornarTrue()
        {
            var resultado = _validator.EmailValido("teste@email.com");

            Assert.True(resultado);
        }

        [Fact]
        public void Email_Invalido_DeveRetornarFalse()
        {
            var resultado = _validator.EmailValido("emailinvalido");

            Assert.False(resultado);
        }

        #endregion

        #region Senha

        [Fact]
        public void Senha_Forte()
        {
            var resultado = _validator.SenhaValida("Senha@123");

            Assert.True(resultado);
        }

        [Fact]
        public void Senha_SemNumero()
        {
            var resultado = _validator.SenhaValida("Senha@@@");

            Assert.False(resultado);
        }

        #endregion

        #region Tamanho do Campo

        [Fact]
        public void TextoDentroDoLimite()
        {
            var resultado = _validator.TamanhoMaximo("abc", 5);

            Assert.True(resultado);
        }

        [Fact]
        public void TextoAcimaDoLimite()
        {
            var resultado = _validator.TamanhoMaximo("abcdef", 5);

            Assert.False(resultado);
        }

        #endregion

    }
}
