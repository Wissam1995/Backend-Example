using RestEase;

namespace WeatherCheckApi.Extensions
{
    public static class Extensions
    {
        public static void RegisterServiceForwarder<T>(this IServiceCollection services, string serviceName) where T : class
        {
            string clientName = typeof(T).ToString();
            RestEaseOptions options = ConfigureOptions(services);
            ConfigureDefaultClient(services, clientName, serviceName, options);
            ConfigureForwarder<T>(services, clientName);
        }

        private static RestEaseOptions ConfigureOptions(IServiceCollection services)
        {
            IConfiguration service;
            using (ServiceProvider provider = services.BuildServiceProvider())
            {
                service = provider.GetService<IConfiguration>();
            }

            services.Configure<RestEaseOptions>(service.GetSection("restEase"));
            return service.GetOptions<RestEaseOptions>("restEase");
        }

        private static void ConfigureDefaultClient(IServiceCollection services, string clientName, string serviceName, RestEaseOptions options)
        {
            services.AddHttpClient(clientName, delegate (HttpClient client)
            {
                RestEaseOptions.Service service = options.Services.SingleOrDefault((RestEaseOptions.Service s) => s.Name.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase));
                if (service == null)
                {
                    throw new RestEaseServiceNotFoundException("RestEase service: '" + serviceName + "' was not found.", serviceName);
                }

                client.BaseAddress = new UriBuilder
                {
                    Scheme = service.Scheme,
                    Host = service.Host,
                    Port = service.Port
                }.Uri;
            });
        }

        public static string Underscore(this string value)
        {
            return string.Concat(value.Select((char x, int i) => (i <= 0 || !char.IsUpper(x)) ? x.ToString() : ("_" + x)));
        }

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            TModel val = new TModel();
            configuration.GetSection(section).Bind(val);
            return val;
        }

        private static void ConfigureForwarder<T>(IServiceCollection services, string clientName) where T : class
        {
            services.AddTransient((IServiceProvider c) => new RestClient(c.GetService<IHttpClientFactory>()!.CreateClient(clientName))
            {
                RequestQueryParamSerializer = new QueryParamSerializer()
            }.For<T>());
        }
    }
}