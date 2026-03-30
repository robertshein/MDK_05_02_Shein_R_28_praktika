using Microsoft.EntityFrameworkCore;
using System;

namespace praktika28_Shein.Classes.Database
{
    public static class Config
    {
        public static readonly string connection =
            "server=192.168.0.111;" +
            "uid=root;" +
            "pwd=root;" +
            "database=TaskManager;";

        public static readonly MySqlServerVersion version = new MySqlServerVersion(new Version(8, 0, 11));
    }
}