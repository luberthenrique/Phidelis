using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhidelisMatricula.Infra.Data.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ALUNO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IDADE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ALUNO", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_MATRICULA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ID_ALUNO = table.Column<long>(type: "bigint", nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    ANO_LETIVO = table.Column<int>(type: "int", nullable: false),
                    DATA_MATRICULA = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OBSERVACAO = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MATRICULA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_MATRICULA_TB_ALUNO_ID_ALUNO",
                        column: x => x.ID_ALUNO,
                        principalTable: "TB_ALUNO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MATRICULA_ID_ALUNO",
                table: "TB_MATRICULA",
                column: "ID_ALUNO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_MATRICULA");

            migrationBuilder.DropTable(
                name: "TB_ALUNO");
        }
    }
}
