using System;
using Hmxs.Toolkit.AudioCenter;
using UnityEngine;
using UnityEngine.Events;
using AudioType = Hmxs.Toolkit.AudioCenter.AudioType;

namespace Game.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onKilled;
        [SerializeField] private float dieFadeTime;
        protected SpriteRenderer Sprite;
        protected Collider2D Collider;
        protected bool IsAlive = true;
        private Timer _timer;
        
        protected virtual void Start()
        {
            Sprite = GetComponent<SpriteRenderer>();
            Collider = GetComponent<CapsuleCollider2D>();
        }

        protected void Die()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect, "怪物死亡音效"));
            IsAlive = false;
            Collider.enabled = false;
            var fadeColor = Sprite.color;
            _timer = Timer.Register(dieFadeTime,
                onUpdate: time =>
                {
                    if (!Sprite) return;
                    Sprite.color = Color.Lerp(fadeColor, Color.clear, time / dieFadeTime);
                },
                onComplete: () =>
                {
                    onKilled?.Invoke();
                    Destroy(gameObject);
                });
        }

        private void OnDestroy()
        {
            _timer?.Cancel();
        }
    }
}