
using System;
using System.Runtime.InteropServices;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ramsey.NET.Extensions
{
    public static class TestDbExtensions
    {
        public static readonly String TestConnectionString =
            "Server=(localdb)\\mssqllocaldb;Database=ramsey_unit;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        public static DbContextOptionsBuilder<T> ConnectRamseyTestServer<T>(this DbContextOptionsBuilder<T> options, IConfiguration config, bool isUnitTest = false) where T : DbContext
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (!isUnitTest && config == null)
                    throw new NullReferenceException("Config cannot be null if is not Unit Test");

                var connectString = isUnitTest ? TestConnectionString : config.GetConnectionString("RamseyDebug");
                return options.UseSqlServer(connectString);
            }
            else
            {
                return options.UseSqlite<T>(isUnitTest ? "Data Source=ramsey-test.db" : config.GetConnectionString("SqliteDebug")).EnableSensitiveDataLogging();
            }
        }
        
        public static DbContextOptionsBuilder ConnectRamseyTestServer(this DbContextOptionsBuilder options, IConfiguration config, bool isUnitTest = false)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (!isUnitTest && config == null)
                    throw new NullReferenceException("Config cannot be null if is not Unit Test");

                var connectString = isUnitTest ? TestConnectionString : config.GetConnectionString("RamseyDebug");
                return options.UseSqlServer(connectString);
            }
            else
            {
                return options.UseSqlite(isUnitTest ? "Data Source=ramsey-test.db" : config.GetConnectionString("SqliteDebug"));
            }
        }

        public static IGlobalConfiguration ConnectHangfireTest(this IGlobalConfiguration hangfire,
            IConfiguration config)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return hangfire.UseSqlServerStorage(config.GetConnectionString("RamseyDebug"));
            }
            else
            {
                return hangfire.UseSQLiteStorage(config.GetConnectionString("SqliteDebug"));
            }
        }
    }
}