using SoundAndMusic.DataBase;
using UnityEngine;

namespace SoundAndMusic
{
    public class SoundAndMusicController : MonoBehaviour
    {
        private static SoundAndMusicController instance = null;

        private const string soundVolumeKey = "SoundVolume";
        private const string backgroundVolumeKey = "BackgroundVolume";

        [Header("General")]
        [SerializeField] private AudioSource backgroundAudioSource = null;
        [SerializeField] private Transform parentSoundsAudioSources = null;
        [SerializeField] private int audioSourcesCount = 1;

        [Header("Volume settings")]
        [SerializeField] private float soundVolumeMax = 1f;
        [SerializeField] private float backgroundVolumeMax = 0.3f;

        private AudioSource[] soundsAudioSources = null;
        private SoundsAndMusicDB soundsAndMusicDB = null;
        private float backgroundMusicVolume = 0f;
        private float soundVolume = 0f;

        public static SoundAndMusicController Instance
        {
            get
            {
                return instance;
            }
        }

        public float BackgroundMusicVolume
        {
            get
            {
                return backgroundMusicVolume;
            }
        }

        public float SoundVolume
        {
            get
            {
                return soundVolume;
            }
        }

        public void Start()
        {
            Init();
        }

        private void Init()
        {
            instance = this;
            soundsAndMusicDB = Resources.Load<SoundsAndMusicDB>("SoundsAndMusicDB");
            backgroundMusicVolume = PlayerPrefs.GetFloat(backgroundVolumeKey, backgroundVolumeMax);
            soundVolume = PlayerPrefs.GetFloat(soundVolumeKey, soundVolumeMax);

            CreateSoundAudioSources();
            DontDestroyOnLoad(gameObject);
        }

        private void CreateSoundAudioSources()
        {
            soundsAudioSources = new AudioSource[audioSourcesCount];

            for (int i = 0; i < audioSourcesCount; i++)
            {
                GameObject audioSource = new GameObject($"AudioSource-{i + 1}");
                audioSource.transform.SetParent(parentSoundsAudioSources);
                soundsAudioSources[i] = audioSource.AddComponent<AudioSource>();
                soundsAudioSources[i].playOnAwake = false;
                soundsAudioSources[i].loop = false;
            }
        }

        #region Playing methods

        public void PlayBackgroundMusic(MusicType musicType)
        {
            foreach (var musicItem in soundsAndMusicDB.MusicItems)
            {
                if (musicItem.MusicType == musicType)
                {
                    backgroundAudioSource.clip = musicItem.AudioClip;
                    break;
                }
            }

            backgroundAudioSource.Play();
        }

        public void PlaySoundAudio(SoundType soundType)
        {
            foreach (var soundItem in soundsAndMusicDB.SoundsItems)
            {
                if (soundItem.SoundType == soundType)
                {
                    for (int i = 0; i < soundsAudioSources.Length; i++)
                    {
                        if (soundsAudioSources[i].isPlaying) continue;

                        soundsAudioSources[i].clip = soundItem.AudioClip;
                        soundsAudioSources[i].Play();
                        return;
                    }

                    var randomAudioSource = soundsAudioSources[Random.Range(0, audioSourcesCount)];
                    randomAudioSource.clip = soundItem.AudioClip;
                    randomAudioSource.Play();
                    return;
                }
            }
        }

        public void StopAllAudioSource()
        {
            backgroundAudioSource.Stop();

            for (int i = 0; i < soundsAudioSources.Length; i++)
            {
                soundsAudioSources[i].Stop();
            }
        }

        #endregion

        #region Volume settings methods

        public void SetSoundVolumeAudioSources(float volume)
        {
            soundVolume = volume;
            PlayerPrefs.SetFloat(soundVolumeKey, soundVolume);

            for (int i = 0; i < soundsAudioSources.Length; i++)
            {
                soundsAudioSources[i].volume = volume;
            }
        }

        public void SetVolumeBackGroundMusic(float volume)
        {
            backgroundMusicVolume = volume;
            PlayerPrefs.SetFloat(backgroundVolumeKey, backgroundMusicVolume);
            backgroundAudioSource.volume = volume;
        }

        #endregion
    }
}