using HighlightPlus2D;
using Hmxs.Toolkit.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Element
{
    public abstract class Interactable : MonoBehaviour
    {
        [Required]
        [SerializeField] private HighlightEffect2D highlightEffect2D;
        protected bool IsPlayerNear;
        protected bool CanInteract;

        protected virtual void OnEnable() =>
            EventCenter.AddListener(EventName.Player.SubmitPressed, OnPlayerPressed);

        protected virtual void OnDisable() =>
            EventCenter.RemoveListener(EventName.Player.SubmitPressed, OnPlayerPressed);


        protected abstract void OnPlayerPressed();
        
        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (CanInteract)
            {
                if (other.CompareTag("Player"))
                {
                    IsPlayerNear = true;
                    highlightEffect2D.SetHighlighted(true);
                }
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (CanInteract)
            {
                if (other.CompareTag("Player"))
                {
                    IsPlayerNear = false;
                    highlightEffect2D.SetHighlighted(false);
                }
            }
        }
    }
}