using System;
using UnityEngine;

namespace Manager
{
    public class Manager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}