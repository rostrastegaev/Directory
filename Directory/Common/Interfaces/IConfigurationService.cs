namespace Common
{
    public interface IConfigurationService
    {
        T GetConfig<T>(string section);
    }
}
