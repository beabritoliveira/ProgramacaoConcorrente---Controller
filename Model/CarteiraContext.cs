using Microsoft.EntityFrameworkCore;

namespace CriptoMoed.Model
{
    //gerenciar a conexão com o banco de dados
    public class CarteiraContext : DbContext  //MoedasContext herda da classe base DbContext
    {
        public CarteiraContext(DbContextOptions<CarteiraContext> options) : base(options)
        {
        }

        public DbSet<Carteira> CarteiraItens { get; set; } = null;
        //Representa uma tabela no banco de dados para a entidade Moedas

    }
}
