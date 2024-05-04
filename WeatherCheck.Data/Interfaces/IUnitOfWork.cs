namespace WeatherCheck.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IWeatherRepository Weather { get; }
    }
}