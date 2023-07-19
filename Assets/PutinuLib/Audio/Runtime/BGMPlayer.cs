using System;
using DG.Tweening;
using UnityEngine;

namespace PutinuLib.Audio
{
    /// <summary>
    /// BGMを再生するクラス
    /// </summary>
    public class BGMPlayer :MonoBehaviour
    {
        private const float DefaultFadeTime = 0.4f;
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private BGMList _bgmList;

        public float Volume { get; private set; } = 0.24f;
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
        public void Play(BGMType bgmType)
        {
            AudioClip audioClip = _bgmList.GetAudioClip(bgmType);
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        /// <summary>
        /// 再生を止める
        /// </summary>
        public void Stop()
        {
            _audioSource.Stop();
        }

        /// <summary>
        /// フェードインする
        /// </summary>
        public void FadeIn(float fadeTime = DefaultFadeTime, Action onCompleted = default)
        {
            _fadeTween?.Kill();
            _fadeTween = DOVirtual.Float(
                from: 0,
                to: Volume,
                duration: fadeTime,
                onVirtualUpdate: currentVolume =>
                {
                    _audioSource.volume = currentVolume;
                }
            );
        }

        /// <summary>
        /// フェードアウトする
        /// </summary>
        public void FadeOut(float fadeTime = DefaultFadeTime, Action onCompleted = default)
        {
            _fadeTween?.Kill();
            _fadeTween = DOVirtual.Float(
                from: _audioSource.volume,
                to: 0,
                duration: fadeTime,
                onVirtualUpdate: currentVolume =>
                {
                    _audioSource.volume = currentVolume;
                }
            );
        }
    }
}