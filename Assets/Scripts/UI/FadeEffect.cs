using System;
using Hmxs.Toolkit.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class FadeEffect : MonoBehaviour
    {
        [Title("SceneStart")]
        [SerializeField] private float fadeSceneStartTime; 
        [SerializeField] private Color fadeSceneStartColor;
        [SerializeField] private UnityEvent onSceneStartFadeStart;
        [SerializeField] private UnityEvent onSceneStartFadeFinish;
        
        [Title("SceneEnd")]
        [SerializeField] private float fadeSceneEndTime;
        [SerializeField] private Color fadeSceneEndColor;
        [SerializeField] private UnityEvent onSceneEndFadeStart;
        [SerializeField] private UnityEvent onSceneEndFadeFinish;

        public static FadeEffect Instance;
        private RawImage _image;

        private void OnEnable()
        {
            EventCenter.AddListener(EventName.UI.SceneStartFadeEffect, SceneStartFadeEffect);
            EventCenter.AddListener(EventName.UI.SceneEndFadeEffect, SceneEndFadeEffect);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener(EventName.UI.SceneStartFadeEffect, SceneStartFadeEffect);
            EventCenter.RemoveListener(EventName.UI.SceneEndFadeEffect, SceneEndFadeEffect);
        }
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Debug.LogWarning("FadeEffect: Multiple Instance");
            
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            _image = GetComponent<RawImage>();
            _image.enabled = false;
        }

        private void SceneStartFadeEffect()
        {
            onSceneStartFadeStart?.Invoke();
            _image.enabled = true;
            _image.color = fadeSceneStartColor;
            Timer.Register(fadeSceneStartTime,
                onUpdate: (time) =>
                {
                    _image.color = Color.Lerp(fadeSceneStartColor, Color.clear, time / fadeSceneStartTime);
                }, 
                onComplete: () =>
                {
                    _image.enabled = false;
                    onSceneStartFadeFinish?.Invoke();
                });
        }

        private void SceneEndFadeEffect()
        {
            onSceneEndFadeStart?.Invoke();
            _image.enabled = true;
            _image.color = Color.clear;
            Timer.Register(fadeSceneStartTime,
                onUpdate: (time) =>
                {
                    _image.color = Color.Lerp(Color.clear, fadeSceneEndColor , time / fadeSceneEndTime);
                }, 
                onComplete: () =>
                {
                    _image.enabled = false;
                    onSceneEndFadeFinish?.Invoke();
                });
        }

        public void Fade(float fadeTime, Color formColor, Color toColor, Action onStart = null, Action onFinish = null)
        {
            onStart?.Invoke();
            _image.enabled = true;
            _image.color = formColor;
            Timer.Register(fadeTime,
                onUpdate: time =>
                {
                    _image.color = Color.Lerp(formColor, toColor, time / fadeTime);
                },
                onComplete: () =>
                {
                    _image.enabled = false;
                    onFinish?.Invoke();
                }, useRealTime: true);
        }
    }
}
