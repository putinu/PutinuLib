﻿using UnityEngine;

namespace PutinuLib.CommonView
{
    /// <summary>
    /// UIであるMonoBehaviourが継承して使う
    /// </summary>
    public abstract class UIMonoBehaviour : MonoBehaviour
    {
        public RectTransform RectTransform => transform as RectTransform;
    }
}