using System.ComponentModel.DataAnnotations;

namespace CriptoMoed.Model
{
    public class Moedas
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }

        public Moedas(int id, string nome, decimal valor)
        {
            Id = id;
            Nome = nome;
            Valor = valor;
        }

        public Moedas()
        {
        }
    }
}
