using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.SoundManagers
{
    public interface test { }
    public enum SoundEffects
    {
        JumpSuper
    }
    public enum Musics
    {
        Mario
    }
    public class SoundManager
    {
        private static object lockObj = new object();
        private static SoundManager instance = null;
        public static SoundManager Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new SoundManager();
                    }
                    return instance;
                }
            }
        }
        public ContentManager Content { get; set; }
        private string SoundFolder { get; } = "Sounds/";
        private string SoundEffectsFolder { get; }
        private string MusicsFolder { get; }
        private Song _currentSong;
        public SoundEffects CurrentSong { get; private set; }
        private Dictionary<Enum, SoundEffect> SoundEffectsDict { get; set; }
        private Dictionary<Enum, Song> SongsDict { get; set; }
        private SoundManager()
        {
            SoundEffectsFolder = $"{SoundFolder}SoundEffects/";
            MusicsFolder = $"{SoundFolder}Musics/";
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        ///Create new instance of Dictionary to replace the one already existing and load sound effects that are used in all stages/worlds.
        /// </summary>
        public void LoadInitialSoundEffects()
        {
            SoundEffectsDict = new Dictionary<Enum, SoundEffect>();
            AddKeyValueToDictionary(SoundEffectsDict, GetKeyValueSound<SoundEffect>(SoundEffects.JumpSuper));
        }

        public void LoadSongs(List<Musics> musics)
        {
            SongsDict = new Dictionary<Enum, Song>();
            foreach (Musics music in musics)
            {
                AddKeyValueToDictionary(SongsDict, GetKeyValueSound<Song>(music));
            }
        }

        private void AddKeyValueToDictionary<Enum,T>(Dictionary<Enum,T> dictionary, KeyValuePair<Enum,T> keyValue)
        {
            dictionary.Add(keyValue.Key, keyValue.Value);
        }

        private KeyValuePair<Enum, T> GetKeyValueSound<T>(Enum soundName)
        {
            return new KeyValuePair<Enum, T>(soundName, Load<T>(soundName));
        }

        private T Load<T>(Enum name)
        {
            return Content.Load<T>(SoundFilePath(name));
        }

        private string SoundFilePath(Enum name)
        {
            return (name.GetType().Name == "SoundEffects" ? SoundEffectsFolder : MusicsFolder) + name.ToString();
        }

        public void SetSong(Musics song, bool playSong = false)
        {
            MediaPlayer.Stop();
            _currentSong = SongsDict[song];
            if (playSong) MediaPlayer.Play(_currentSong);
        }

        public void PlaySoundEffect(SoundEffects soundEffects)
        {
            if (SoundEffectsDict.ContainsKey(soundEffects))
            {
                SoundEffectsDict[soundEffects].Play();
            }
        }
    }
}
