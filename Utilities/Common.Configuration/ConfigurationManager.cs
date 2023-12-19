namespace Common.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        public string GetEnvironmentVariable(string variableName)
        {
            var environmentVar = Environment.GetEnvironmentVariable(variableName);
            if (string.IsNullOrWhiteSpace(environmentVar))
            {
                throw new ApplicationException($"There is no value for the environment variable {variableName} on this server {Environment.MachineName} - {Environment.OSVersion}");
            }

            return environmentVar.ToString();
        }
    }
}
