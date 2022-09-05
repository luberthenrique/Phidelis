using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhidelisMatricula.Domain.Entities;

namespace PhidelisMatricula.Infra.Data.Mappings
{
    public class AlunoMapping: IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("TB_ALUNO");

            builder.HasKey(k => k.Id);
            builder.Property(c => c.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Idade)
                .HasColumnName("IDADE")
                .IsRequired();
        }
    }
}
