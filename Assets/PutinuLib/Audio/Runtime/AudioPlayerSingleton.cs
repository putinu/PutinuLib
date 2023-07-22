using UnityEngine;

namespace PutinuLib.Audio
{
    /// <summary>
    /// monoBehaviour用Singletonクラス
    /// </summary>
    public abstract class AudioPlayerSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T s_instance;

        /// <summary>
        /// 対象クラスのインスタンス
        /// </summary>
        public static T Instance
        {
            get
            {
                if (s_instance != null) return s_instance;
                
                s_instance = FindObjectOfType<T>();
                if (s_instance == null)
                {
                    Debug.LogError(typeof(T) + " をアタッチしているGameObjectがないよ！");
                }

                return s_instance;
            }
        }

        protected virtual void Awake()
        {
            if (this != Instance)
            {
                Destroy(this);
            }
        }
    }
}