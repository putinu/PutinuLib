using DG.Tweening;
using UnityEngine;

namespace PutinuLib.Audio
{
    /// <summary>
    /// SEを再生するクラス
    /// </summary>
    public class SEPlayer : AudioPlayerSingleton<SEPlayer>
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SEList _seList;

        public float Volume { get; private set; } = 0.4f;
        private Tweener _fadeTween;

        private void OnDestroy()
        {
            _fadeTween?.Kill();
        }
        
        private void Start()
        {
            SetVolume(Volume);
        }

        /// <summary>
        /// 音量を設定する
        /// </summary>
        /// <param name="volume">音量</param>
        public void SetVolume(float volume)
        {
            float clampedVolume = Mathf.Clamp01(volume);
            Volume = clampedVolume;
            _audioSource.volume = clampedVolume;
        }

        /// <summary>
        /// 再生する
        /// </summary>
        public void Play(SEType seType)
        {
            AudioClip audioClip = _seList.GetAudioClip(seType);
            _audioSource.volume = Volume;
            _audioSource.PlayOneShot(audioClip);
        }

        /// <summary>
        /// 再生を止める
        /// </summary>
        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}