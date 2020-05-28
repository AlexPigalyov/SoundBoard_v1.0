using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Reflection;
using NAudio.Wave;
using Services.Managers;

namespace Services
{
    public class ConfigurationService
    {
        private List<WaveInCapabilities> _sources;
        private IniFileManager _iniFile;
        private string _currentDirectory;
        private string _iniFilePath;

        public ConfigurationService()
        {
            _currentDirectory = Environment.CurrentDirectory;

            Directory.CreateDirectory(Path
                .Combine(_currentDirectory, "Config"));

            _iniFilePath = Path
                .Combine(_currentDirectory, @"Config\config.ini");

            GetSources();

            if (File.Exists(_iniFilePath))
            {
                _iniFile = new IniFileManager(_iniFilePath);

                _iniFile.Write("MicrophoneName", "None", "Microphone");
                _iniFile.Write("MicrophoneChannels", "None");
                _iniFile.Write("MicrophoneIndex", "None");

                _iniFile.Write("AudioName", _sources[0].ProductName, "Audio");
                _iniFile.Write("AudioChannels", _sources[0].Channels.ToString());
                _iniFile.Write("AudioIndex", 0.ToString());
            }
            else
            {
                _iniFile = new IniFileManager(_iniFilePath);
            }
        }

        public List<WaveInCapabilities> GetSources()
        {
            _sources = new List<WaveInCapabilities>();

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                _sources.Add(WaveIn.GetCapabilities(i));
            }

            return _sources;
        }

        public void SetConfiguration(ConfigurationModel configuration)
        {
            _iniFile.Write("MicrophoneName", configuration.SelectedMic.Name, "Microphone");
            _iniFile.Write("MicrophoneChannels", configuration.SelectedMic.Channels.ToString());
            _iniFile.Write("MicrophoneIndex", configuration.SelectedMic.Index.ToString());

            _iniFile.Write("AudioName", configuration.SelectedAudio.Name, "Audio");
            _iniFile.Write("AudioChannels", configuration.SelectedAudio.Channels.ToString());
            _iniFile.Write("AudioIndex", configuration.SelectedAudio.Index.ToString());
        }

        public ConfigurationModel GetConfiguration()
        {
            string microphoneName = _iniFile.Read("MicrophoneName");
            int microphoneChannels = int.Parse(_iniFile.Read("MicrophoneChannels"));
            int microphoneIndex = int.Parse(_iniFile.Read("MicrophoneIndex"));

            string audioName = _iniFile.Read("AudioName");
            int audioChannels = int.Parse(_iniFile.Read("AudioChannels"));
            int audioIndex = int.Parse(_iniFile.Read("AudioIndex"));

            return new ConfigurationModel
            {
                SelectedMic = new SourceModel
                {
                    Name = microphoneName,
                    Channels = microphoneChannels,
                    Index = microphoneIndex
                },
                SelectedAudio = new SourceModel
                {
                    Name = audioName,
                    Channels = audioChannels,
                    Index = audioIndex
                }
            };
        }
    }
}
