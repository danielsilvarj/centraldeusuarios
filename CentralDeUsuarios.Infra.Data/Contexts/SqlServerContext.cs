using CentralDeUsuarios.Domain.Entities;
using CentralDeUsuarios.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.Infra.Data.Contexts
{
    public class SqlServerContext : DbContext
    {

        public SqlServerContext(DbContextOptions<SqlServerContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
        }

        public DbSet<Usuario> Usuario { get; set; }
    }
}
