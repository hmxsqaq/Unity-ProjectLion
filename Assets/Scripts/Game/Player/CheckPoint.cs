using System;
using Fungus;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    public class CheckPoint : MonoBehaviour
    {
        [SerializeField] private UnityEvent onPlayerEnter;

        private Flowchart _flowchart;

        private void Start()
        {
            _flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.GetComponent<PlayerController>().GetCheckPoint() == transform) return;
            onPlayerEnter?.Invoke();
        }

        public void ShowCheckPoint() => _flowchart.ExecuteBlock("CheckPointInfo");
    }
}