using System.Collections.Generic;
using UnityEngine;
using System;

namespace SoundAndMusic.DataBase
{
    [CreateAssetMenu(fileName = "SoundsAndMusicDB", menuName = "ScriptableObjects/DataBase/SoundsAndMusicDB", order = 1)]
    public class SoundsAndMusicDB : ScriptableObject
    {
        [SerializeField] private List<SoundsItem> soundsItems = new List<SoundsItem>();
        [SerializeField] private List<MusicItem> musicItems = new List<MusicItem>();

        public List<SoundsItem> SoundsItems
        {
            get
            {
                return soundsItems;
            }
        }

        public List<MusicItem> MusicItems
        {
            get
            {
                return musicItems;
            }
        }
    }

    [Serializable]
    public class SoundsItem
    {
        public SoundType SoundType = default;
        public AudioClip AudioClip = default;
    }

    [Serializable]
    public class MusicItem
    {
        public MusicType MusicType = default;
        public AudioClip AudioClip = default;
    }
}

namespace SoundAndMusic
{
    public enum SoundType
    {
        None = 0
    }

    public enum MusicType
    {
        None = 0
    }
}