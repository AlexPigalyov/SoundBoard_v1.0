using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Models;
using Services;
using Services.Abstract;
using Services.Managers;
using WMPLib;

namespace Shell.ViewModels {
    public class ShellViewModel : Screen
    {
        private readonly ISoundService _soundService;

        private bool _hookActivate = false;
        private string _selectedFilename;
        private string _selectedKey;
        private KeyBoardHookManager _keyBoardHook;
        private bool _isPlaying = false;
        private List<string> _notDeletedSound = new List<string>();

        private string _musicPath = Path
            .Combine(Environment.CurrentDirectory, "Music");

        #region Bindings
        public Visibility VolumeSliderIsVisible { get; set; }
        public string Title { get; set; } = "SoundBoard";
        public int Position { get; set; } = 0;
        public string SetHookAlert { get; set; } = string.Empty;
        public int SelectedSoundLong { get; set; } = 0;
        public ObservableCollection<SoundModel> Music { get; set; }
        public bool FooterIsVisible = false;
        public SoundModel SelectedSound { get; set; }
        public float Volume { get; set; } = 1;
        public int LongOfSong { get; set; } = 0;
        public float StartPosition { get; set; } = 0;
        #endregion

        public ShellViewModel(ISoundService soundService)
        {
            #region Initialization some property and install keyboard global hook for hotkeys


            _selectedFilename = string.Empty;
            _selectedKey = string.Empty;

            Music = new ObservableCollection<SoundModel>();

            _keyBoardHook = new KeyBoardHookManager();

            _soundService = soundService;

            _keyBoardHook.KeyDown += keyboardHook_KeyDown;
            _keyBoardHook.Install();

            Directory.CreateDirectory(_musicPath);

            Task.Run(async () =>
            {
                var music = await _soundService.GetAsync();
                Music = new ObservableCollection<SoundModel>(music);

                List<string> files = Directory.GetFiles(_musicPath).ToList();

                files = files.Except(Music.Select(x => x.FullPath)).ToList();

                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            });

            #endregion

            Task.Run(async () =>
            {
                while (_isPlaying)
                {
                    await Task.Delay(1000);
                    StartPosition += 1;
                }
            });
        }

        private void keyboardHook_KeyDown(KeyBoardHookManager.VKeys key)
        {
            if (_hookActivate)
            {
                _selectedKey = key
                    .ToString()
                    .Replace("KEY_", string.Empty)
                    .Replace("OEM_", string.Empty);
            }
            else
            {
                foreach (var music in Music)
                {
                    string keyString = key
                        .ToString()
                        .Replace("KEY_", string.Empty)
                        .Replace("OEM_", string.Empty);

                    if (keyString == music.HotKey)
                    {
                        _soundService.PlaySound(music.FullPath, Volume);
                    }
                }
            }

        }


        public async Task DeleteSelectedSound()
        {
            
            if (SelectedSound != null)
            {
                if(_soundService.PlayingSoundFullPath == SelectedSound.FullPath)
                {
                    _isPlaying = false;
                    _soundService.StopSound();
                }
                await _soundService.RemoveByIdAsync(SelectedSound.Id);
                Music.Remove(SelectedSound);
            }
        }

        public void SliderValueChanged(RoutedPropertyChangedEventArgs<float> e)
        {
            _soundService.SetVolume(Volume);
        }

        public void DurationPositionChanged(RoutedPropertyChangedEventArgs<float> e)
        {
            _soundService.SetPosition(Convert.ToDouble(StartPosition));
        }

        public void PlaySound()
        {
            FooterIsVisible = true;
            _isPlaying = true;
            _soundService.PlaySound(SelectedSound?.FullPath, Volume);
        }

        public void FilePreviewDragEnter(DragEventArgs e)
        {
            e.Handled = true;
        }

        public void CloseProgram()
        {
            foreach(var file in _notDeletedSound)
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
            }
            _isPlaying = false;
            TryClose();
        }

        public void StopSound()
        {
            _isPlaying = false;
            FooterIsVisible = false;
            _soundService.StopSound();
        }

        public async void FileDropped(DragEventArgs e)
        {
            var dragFileList = ((DataObject) e.Data)
                .GetFileDropList();


            foreach (var file in dragFileList)
            {
                if (Path.GetExtension(file) != ".mp3")
                {
                    break;
                }

                _selectedKey = string.Empty;
                _selectedFilename = Path.GetFileName(file);
                _hookActivate = true;
                VolumeSliderIsVisible = Visibility.Hidden;
                
                

                await Task.Run(() =>
                {
                    SetHookAlert = "”становите гор€чую клавишу!";
                    while (_selectedKey == string.Empty) { }

                    _hookActivate = false;
                    VolumeSliderIsVisible = Visibility.Visible;
                    SetHookAlert = string.Empty;
                });

                if (_selectedKey != string.Empty)
                {
                    string filePath = Path
                        .Combine(_musicPath, _selectedFilename);
                    
                    if (!File.Exists(filePath))
                    {
                        try
                        {
                            File.Copy(file, filePath);
                        }
                        catch { }
                    }


                    SoundModel sound = new SoundModel
                    {
                        HotKey = _selectedKey
                            .ToString()
                            .Replace("KEY_", string.Empty)
                            .Replace("OEM_", string.Empty),

                        SoundName = _selectedFilename
                            .Substring(0, _selectedFilename.Length - 4),

                        FullPath = filePath,

                        Id = (Music.Count + 1)
                    };

                    Music.Add(sound);
                    _selectedFilename = string.Empty;

                    await _soundService.CreateAsync(sound);
                }
            }
        }
    }
}
