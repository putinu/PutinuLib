using System.Collections.Generic;
using UnityEngine;

namespace PutinuLib.Audio
{
    /// <summary>
    /// ScriptableObjectから定義するBGMリスト
    /// </summary>
    [CreateAssetMenu(menuName = "PutinuLib/Audio/BGMList")]
    public class BGMList : ScriptableObject
    {
        [SerializeField] private List<AudioClip> _bgmList;

        /// <summary>
        /// 対象の種類のAudioClipを取得する
        /// </summary>
        public AudioClip GetAudioClip(BGMType bgmType)
        {
            int index = (int) bgmType;
            return _bgmList[index];
        }
    }
}