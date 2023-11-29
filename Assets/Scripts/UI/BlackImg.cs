using System;
using UnityEngine;

namespace UI
{
    public class BlackImg : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        }
    }
}