﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhidelisMatricula.Infra.Data.Context;

namespace PhidelisMatricula.Infra.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("PhidelisMatricula.Domain.Entities.Aluno", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    b.Property<int>("Idade")
                        .HasColumnType("int")
                        .HasColumnName("IDADE");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("NOME");

                    b.HasKey("Id");

                    b.ToTable("TB_ALUNO");
                });

            modelBuilder.Entity("PhidelisMatricula.Domain.Entities.Matricula", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    b.Property<int>("AnoLetivo")
                        .HasColumnType("int")
                        .HasColumnName("ANO_LETIVO");

                    b.Property<DateTime>("DataMatricula")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("DATA_MATRICULA");

                    b.Property<long>("IdAluno")
                        .HasColumnType("bigint")
                        .HasColumnName("ID_ALUNO");

                    b.Property<string>("Observacao")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("OBSERVACAO");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("STATUS");

                    b.HasKey("Id");

                    b.HasIndex("IdAluno");

                    b.ToTable("TB_MATRICULA");
                });

            modelBuilder.Entity("PhidelisMatricula.Domain.Entities.Matricula", b =>
                {
                    b.HasOne("PhidelisMatricula.Domain.Entities.Aluno", "Aluno")
                        .WithMany("Matriculas")
                        .HasForeignKey("IdAluno")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Aluno");
                });

            modelBuilder.Entity("PhidelisMatricula.Domain.Entities.Aluno", b =>
                {
                    b.Navigation("Matriculas");
                });
#pragma warning restore 612, 618
        }
    }
}
