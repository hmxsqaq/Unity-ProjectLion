using System;
using Fungus;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Element
{
    public class StoneLion : Interactable
    {
        [Required]
        [SerializeField] private Flowchart flowchart;
        [SerializeField] private string firstMeetLion;
        [SerializeField] private string firstInteractWithLion;

        private BoxCollider2D _boxCollider;
        
        private void Start()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            CanInteract = false;
        }

        public bool IsPlayerFacingRight()
        {
            var player = GameObject.FindWithTag("Player").transform.localScale.x;
            return player > 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!CanInteract && other.CompareTag("Player"))
            {
                ExecuteFungusBlock(firstMeetLion);
            }
        }

        public void SetCanInterAct(bool canInteract)
        {
            CanInteract = canInteract;
        }
        
        protected override void OnPlayerPressed()
        {
            if (!IsPlayerNear) return;
            ExecuteFungusBlock(firstInteractWithLion);
            _boxCollider.enabled = false;
        }

        private void ExecuteFungusBlock(string blockName)
        {
            if (flowchart.HasBlock(blockName)) 
                flowchart.ExecuteBlock(blockName);
            else
                Debug.LogError($"{blockName} not exist");
        }
    }
}