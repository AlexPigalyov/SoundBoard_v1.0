using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using NAudio.Wave;

namespace Services.Abstract
{
    public interface ISoundService
    {
        string PlayingSoundFullPath { get; set; }
        WaveStream Wave { get; set; }
        void SetPosition(double seconds);
        void PlaySound(string path, float volume = 1);
        void StopSound();
        void SetVolume(float volume);
        Task<IEnumerable<SoundModel>> GetAsync();
        Task RemoveByIdAsync(int id);
        Task RemoveAsync(SoundModel sound);
        Task CreateAsync(SoundModel sound);
    }
}
