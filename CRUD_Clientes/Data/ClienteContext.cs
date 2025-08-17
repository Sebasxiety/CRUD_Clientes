using Microsoft.EntityFrameworkCore;
using CRUD_Clientes.Models;

namespace CRUD_Clientes.Data
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; } = default!;
    }
}
