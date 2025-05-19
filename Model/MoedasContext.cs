using Microsoft.EntityFrameworkCore;

namespace CriptoMoed.Model
{
    //gerenciar a conexão com o banco de dados
    public class MoedasContext : DbContext  //MoedasContext herda da classe base DbContext
    {
        public MoedasContext(DbContextOptions<MoedasContext> options) : base(options)
        {
        }

        public DbSet<Moedas> MoedasItens { get; set; } = null;
        //Representa uma tabela no banco de dados para a entidade Moedas

    }
}
