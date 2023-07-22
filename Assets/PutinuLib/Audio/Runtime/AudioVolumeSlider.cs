using System;
using UnityEngine;
using UnityEngine.UI;

namespace PutinuLib.Audio
{
    /// <summary>
    /// サウンド音量を設定するためのクラス
    /// </summary>
    public class SoundVolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private AudioType _audioType;

        private void Start()
        {
            SetSliderValue();
            _slider.onValueChanged.AddListener(SetVolume);
        }

        private void SetSliderValue()
        {
            _slider.value = _audioType switch
            {
                AudioType.BGM => BGMPlayer.Instance.Volume,
                AudioType.SE => SEPlayer.Instance.Volume,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void SetVolume(float volume)
        {
            switch (_audioType)
            {
                case AudioType.BGM:
                    BGMPlayer.Instance.SetVolume(volume);
                    break;
                case AudioType.SE:
                    SEPlayer.Instance.SetVolume(volume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}