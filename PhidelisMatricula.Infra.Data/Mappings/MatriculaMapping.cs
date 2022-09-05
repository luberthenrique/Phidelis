using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhidelisMatricula.Domain.Entities;

namespace PhidelisMatricula.Infra.Data.Mappings
{
    public class MatriculaMapping: IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.ToTable("TB_MATRICULA");

            builder.HasKey(k => k.Id);
            builder.Property(c => c.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.IdAluno)
                .HasColumnName("ID_ALUNO");

            builder.Property(c => c.Status)
                .HasColumnName("STATUS");

            builder.Property(c => c.AnoLetivo)
                .HasColumnName("ANO_LETIVO");

            builder.Property(c => c.DataMatricula)
                .HasColumnName("DATA_MATRICULA");

            builder.Property(c => c.Observacao)
                .HasColumnName("OBSERVACAO")
                .HasMaxLength(300)
                .IsRequired(false);

            builder.HasOne(o => o.Aluno)
               .WithMany(m => m.Matriculas)
               .HasForeignKey(f => f.IdAluno)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
