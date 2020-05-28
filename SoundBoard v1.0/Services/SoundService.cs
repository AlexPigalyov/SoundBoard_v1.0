using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Data.Repositories.Abstract;
using Enteties;
using Mapster;
using Models;
using NAudio.Wave;
using Services.Abstract;

namespace Services
{
    public class SoundService : ISoundService
    {
        public string PlayingSoundFullPath { get; set; }

        private static WaveOutEvent _outputDevice;
        public  WaveStream Wave { get; set; }
        private ISoundRepository _soundRepository;

        public SoundService(ISoundRepository soundRepository)
        {
            _soundRepository = soundRepository ?? throw new ArgumentNullException(nameof(soundRepository));
            _outputDevice = new WaveOutEvent();
            _outputDevice.PlaybackStopped += _outputDevice_PlaybackStopped;
        }

        private void _outputDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            PlayingSoundFullPath = string.Empty;
        }

        ~SoundService()
        {
            _outputDevice.Dispose();
        }

        public async Task RemoveByIdAsync(int id)
        {
            await _soundRepository.RemoveByIdAsync(id);
        }

        public async Task CreateAsync(SoundModel sound)
        {
            await _soundRepository.CreateAsync(sound.Adapt<SoundEntity>());
        }

        public async Task RemoveAsync(SoundModel sound)
        {
            await _soundRepository.RemoveAsync(sound.Adapt<SoundEntity>());
        }

        public async Task<IEnumerable<SoundModel>> GetAsync()
        {
            var sounds = await _soundRepository.GetAsync();
            return sounds.Adapt<IEnumerable<SoundModel>>();
        }

        public void PlaySound(string path, float volume = 1)
        {
            PlayingSoundFullPath = path;
            string extension = Path.GetExtension(path);

            if (extension == ".mp3")
            {
                Wave = new Mp3FileReader(path);
            }
            else if (extension == ".wav")
            {
                Wave = new WaveFileReader(path);
            }
            else
            {
                return;
            }

            SetVolume(volume);

            _outputDevice.Stop();

            _outputDevice.Init(Wave);

            _outputDevice.Play();
        }

        private WaveStream SetPositionByBytes(WaveStream strm, long position)
        {
            // distance from block boundary (may be 0)
            long adj = position % strm.WaveFormat.BlockAlign;
            // adjust position to boundary and clamp to valid range
            long newPos = Math.Max(0, Math.Min(strm.Length, position - adj));
            // set playback position
            strm.Position = newPos;

            return strm;
        }

        // Set playback position of WaveStream by seconds
        public void SetPosition(double seconds)
        {
            Wave = SetPositionByBytes(Wave, (long)seconds * Wave.WaveFormat.AverageBytesPerSecond);

            _outputDevice.Stop();
            _outputDevice.Init(Wave);
            _outputDevice.Play();
        }

        public void StopSound()
        {
            PlayingSoundFullPath = string.Empty;
            _outputDevice.Stop();
        }

        public void SetVolume(float volume = 1)
        {
            _outputDevice.Volume = volume;
        }
    }
}
