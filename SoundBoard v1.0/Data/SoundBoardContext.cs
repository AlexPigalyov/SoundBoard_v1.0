using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.IO;
using System.Reflection;
using Enteties;
using SQLite.CodeFirst;

namespace Data
{
    public class SoundBoardContext : DbContext
    {
        private static readonly SQLiteConnection connection = new SQLiteConnection()
        {
            ConnectionString = new SQLiteConnectionStringBuilder
            {
                DataSource = Path.Combine(Environment.CurrentDirectory, @"Data\soundboard.db"),
                ForeignKeys = true
            }.ConnectionString
        };

        public SoundBoardContext() : base(connection, true)
        {
            Directory.CreateDirectory(Path
                .Combine(Environment.CurrentDirectory, "Data"));
        }

        public DbSet<SoundEntity> Sounds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<SoundBoardContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }

    public class SQLiteConfiguration : DbConfiguration
    {
        public SQLiteConfiguration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
}
