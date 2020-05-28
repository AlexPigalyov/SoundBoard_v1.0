using System.Collections.Generic;
using Models;
using NAudio.Wave;

namespace Services.Abstract
{
    public interface IConfigurationService
    {
        void SetConfiguration(ConfigurationModel configuration);
        ConfigurationModel GetConfiguration();
        List<WaveInCapabilities> GetSources();
    }
}
