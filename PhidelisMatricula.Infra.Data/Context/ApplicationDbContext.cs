using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using PhidelisMatricula.Domain.Entities;
using PhidelisMatricula.Infra.Data.Mappings;

namespace PhidelisMatricula.Infra.Data.Context
{
    public class ApplicationDbContext : DbContext
    {    
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfiguration(new AlunoMapping());
            modelBuilder.ApplyConfiguration(new MatriculaMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
