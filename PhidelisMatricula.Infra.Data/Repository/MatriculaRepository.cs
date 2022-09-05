using Microsoft.EntityFrameworkCore;
using PhidelisMatricula.Domain.Entities;
using PhidelisMatricula.Domain.Entities.Repository;
using PhidelisMatricula.Infra.Data.Context;
using System.Data;
using System.Threading.Tasks;

namespace PhidelisMatricula.Infra.Data.Repository
{
    public class MatriculaRepository : Repository<Matricula>, IMatriculaRepository
    {
        public MatriculaRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task Truncate()
        {
            var connection = Db.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();

            command.CommandText = "SET FOREIGN_KEY_CHECKS = 0";              
            await command.ExecuteScalarAsync();

            command.CommandText = "TRUNCATE TABLE TB_MATRICULA";
            await command.ExecuteScalarAsync();

            command.CommandText = "SET FOREIGN_KEY_CHECKS = 1";
            await command.ExecuteScalarAsync();

        }
    }
}
