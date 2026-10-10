using System.Collections;
using UnityEngine;

namespace Game.Core {
    /// <summary>
    /// Sistema de audio 2D y 3D con soporte de fundido cruzado para música de tensión.
    /// </summary>
    public class AudioSystem : StaticInstance<AudioSystem> {
        [Header("Fuentes de Audio")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundsSource;
        [SerializeField] private AudioSource _spatialSoundsSource;

        protected override void Awake() {
            base.Awake();
            EnsureAudioSources();
        }

        private void EnsureAudioSources() {
            if (_musicSource == null) {
                var musicObj = new GameObject("MusicSource");
                musicObj.transform.SetParent(transform);
                _musicSource = musicObj.AddComponent<AudioSource>();
                _musicSource.loop = true;
            }
            if (_soundsSource == null) {
                var sfxObj = new GameObject("SFXSource");
                sfxObj.transform.SetParent(transform);
                _soundsSource = sfxObj.AddComponent<AudioSource>();
            }
            if (_spatialSoundsSource == null) {
                var spatialObj = new GameObject("SpatialSFXSource");
                spatialObj.transform.SetParent(transform);
                _spatialSoundsSource = spatialObj.AddComponent<AudioSource>();
                _spatialSoundsSource.spatialBlend = 1f;
            }
        }

        public void PlayMusic(AudioClip clip, float fadeDuration = 0.5f) {
            if (clip == null || (_musicSource != null && _musicSource.clip == clip)) return;
            StartCoroutine(CrossFadeMusicRoutine(clip, fadeDuration));
        }

        private IEnumerator CrossFadeMusicRoutine(AudioClip nextClip, float duration) {
            if (_musicSource == null) yield break;

            float startVol = _musicSource.volume > 0 ? _musicSource.volume : 1f;
            for (float t = 0; t < duration; t += Time.deltaTime) {
                _musicSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
                yield return null;
            }
            _musicSource.Stop();
            _musicSource.clip = nextClip;
            _musicSource.Play();
            for (float t = 0; t < duration; t += Time.deltaTime) {
                _musicSource.volume = Mathf.Lerp(0f, startVol, t / duration);
                yield return null;
            }
            _musicSource.volume = startVol;
        }

        public void PlaySound(AudioClip clip, float volume = 1f) {
            if (clip != null && _soundsSource != null) {
                _soundsSource.PlayOneShot(clip, volume);
            }
        }

        public void PlaySound3D(AudioClip clip, Vector3 worldPosition, float volume = 1f) {
            if (clip != null && _spatialSoundsSource != null) {
                _spatialSoundsSource.transform.position = worldPosition;
                _spatialSoundsSource.PlayOneShot(clip, volume);
            }
        }
    }
}
