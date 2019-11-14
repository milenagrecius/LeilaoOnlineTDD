using Alura.LeilaoOnline.Core;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoTerminaPregao
    {
        [Theory]
        [InlineData(1200, 1250, new double[] { 800, 1150, 1400, 1250 })]
        public void RetornaValorSuperiorMaisProximoDadoLeilaoNessaModalidade(
            double valorDestino,
            double valorEsperado,
            double[] ofertas)
        {
            //Arranje
            IModalidadeAvaliacao modalidade = 
                new OfertaSuperiorMaisProxima(valorDestino);
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();
            for (int i = 0; i < ofertas.Length; i++)
            {
                if ((i % 2 == 0))
                {
                    leilao.RecebeLance(fulano, ofertas[i]);
                } else
                {
                    leilao.RecebeLance(maria, ofertas[i]);
                }
            }

            //Act
            leilao.TerminaPregao();

            //Assert
            Assert.Equal(valorEsperado, leilao.Ganhador.Valor);

        }

        [Theory]
        [InlineData(1200, new double[] { 800, 900, 1000, 1200 })]
        [InlineData(1000, new double[] { 800, 900, 1000, 990 })]
        [InlineData(800, new double[] { 800 })]
        public void RetornaMaiorValorDadoLeilaoComPeloMenosUmLance(
            double valorEsperado, 
            double[] ofertas)
        {
            //Arranje - cenário
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);
            leilao.IniciaPregao();
            for (int i = 0; i < ofertas.Length; i++)
            {
                var valor = ofertas[i];
                if ((i % 2) == 0)
                {
                    leilao.RecebeLance(fulano, valor);
                }
                else
                {
                    leilao.RecebeLance(maria, valor);
                }
            }

            //Act - método sob teste
            leilao.TerminaPregao();

            //Assert
            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);

        }

        [Fact]
        public void LancaInvalidOperationExceptionDadoPregaoNaoIniciado()
        {
            //Arranje - cenário
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            
            //Assert
            var excecaoObtida = Assert.Throws<System.InvalidOperationException>(
                //Act - método sob teste
                () => leilao.TerminaPregao()
            );

            var msgEsperada = "Não é possível terminar o pregão sem que ele tenha começado. Para isso, utilize o método IniciaPregao().";
            Assert.Equal(msgEsperada, excecaoObtida.Message);
        }

        [Fact]
        public void RetornaZeroDadoLeilaoSemLances()
        {
            //Arranje - cenário
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            leilao.IniciaPregao();

            //Act - método sob teste
            leilao.TerminaPregao();

            //Assert
            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
