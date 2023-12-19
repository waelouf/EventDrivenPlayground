namespace Common.Configuration;

public interface IConfigurationManager
{
    string GetEnvironmentVariable(string variableName);
}
