using System;

namespace Services.Common
{
    /*
    export PORT=8082
    export RABBIT_HOST=192.168.99.100
    export RABBIT_PORT=32789
    export POSTGRES="Server=192.168.99.100;Port=32792;Database=postgres;User Id=postgres;"
    */   
    public class Env
    {
        public static int Port { get { return int.Parse(Environment.GetEnvironmentVariable("PORT")); } }
        public static string Rabbit_Host { get { return Environment.GetEnvironmentVariable("RABBIT_HOST"); } }
        public static int Rabbit_Port { get { return int.Parse(Environment.GetEnvironmentVariable("RABBIT_PORT")); } }
        public static string Postgres { get { return Environment.GetEnvironmentVariable("POSTGRES"); } }
    }
}
