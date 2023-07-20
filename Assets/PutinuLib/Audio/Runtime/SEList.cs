using System.Collections.Generic;
using UnityEngine;

namespace PutinuLib.Audio
{
    /// <summary>
    /// ScriptableObjectから定義するSEリスト
    /// </summary>
    [CreateAssetMenu(menuName = "PutinuLib/Audio/SEList")]
    public class SEList : ScriptableObject
    {
        [SerializeField] private List<AudioClip> _seList;

        /// <summary>
        /// 対象の種類のAudioClipを取得する
        /// </summary>
        /// <param name="seType"></param>
        /// <returns></returns>
        public AudioClip GetAudioClip(SEType seType)
        {
            int index = (int) seType;
            return _seList[index];
        }
    }
}