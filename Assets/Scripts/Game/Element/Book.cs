using System;
using Game.Player;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Element
{
    public class Book : Interactable
    {
        [SerializeField] private GameObject book;
        [SerializeField] private GameObject button;
        [SerializeField] private GameObject textObj;
        [SerializeField] private UnityEvent onOpen;

        private bool _isOpen;
        
        private void Start()
        {
            CanInteract = true;
        }

        protected override void OnPlayerPressed()
        {
            if (!IsPlayerNear) return;
            BookInteract(true);
        }

        public void BookInteract(bool enable)
        {
            InputManager.Instance.SetCanAttack(false);
            Time.timeScale = enable ? 0 : 1;
            onOpen?.Invoke();
            button.SetActive(enable);
            book.SetActive(enable);
            textObj.SetActive(enable);
        }
    }
}