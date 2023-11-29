using Game.Player;
using Hmxs.Toolkit.AudioCenter;
using UnityEngine;
using UnityEngine.Events;
using AudioType = Hmxs.Toolkit.AudioCenter.AudioType;

namespace Game.Element
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private UnityEvent onGain;
        [SerializeField] private float speed;
        [SerializeField] private float time;

        private void Start()
        {
            int i = 1;
            this.AttachTimer(time,onUpdate: f =>
            {
                transform.Translate(Vector3.up * (Time.deltaTime * speed * i));
            },onComplete: () =>
            {
                i *= -1;
            },isLooped: true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                onGain?.Invoke();
                AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect, "捡绣球音效"));
                PlayerInfo.Point += 1;
                Destroy(gameObject);
            }
        }
    }
}