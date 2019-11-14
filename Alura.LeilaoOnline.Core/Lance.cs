namespace Alura.LeilaoOnline.Core
{
    public class Lance
    {
        public Interessada Cliente { get; }
        public double Valor { get; }

        public Lance(Interessada cliente, double valor)
        {
            if (valor<0)
            {
                throw new System.ArgumentException("Valor do lance deve ser igual ou maior que zero.");
            }
            Cliente = cliente;
            Valor = valor;
        }
    }
}
