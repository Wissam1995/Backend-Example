namespace WeatherCheckApi.Extensions
{
    public class RestEaseOptions
    {
        public class Service
        {
            public string Name { get; set; }

            public string Scheme { get; set; }

            public string Host { get; set; }

            public int Port { get; set; }
        }

        public string LoadBalancer { get; set; }

        public IEnumerable<Service> Services { get; set; }
    }
}
