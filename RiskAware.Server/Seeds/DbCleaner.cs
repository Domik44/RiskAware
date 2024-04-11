using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;

namespace RiskAware.Server.Seeds
{
    public static class DbCleaner
    {
        /// <summary>
        /// Deletes the entire database
        /// </summary>
        /// <param name="context">DbContext</param>
        public static void DeleteEntireDb(AppDbContext context)
        {
            context.Database.EnsureDeleted();
        }

        /// <summary>
        /// This method truncates all tables in the database
        /// </summary>
        /// <param name="context">DbContext</param>
        public static void TruncateAllTablesData(AppDbContext context)
        {
            // todo fix specific order with foreign keys constraints
            var entities = typeof(AppDbContext).GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(p => p.PropertyType.GetGenericArguments().First());

            foreach (var entity in entities)
            {
                string tableName = context.Model.FindEntityType(entity).GetTableName();
                //Console.WriteLine(tableName);
                context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE [{0}] NOCHECK CONSTRAINT ALL';", tableName);
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE [{0}];", tableName);
                context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE [{0}] WITH CHECK CHECK CONSTRAINT ALL';", tableName);
            }
        }
    }
}
