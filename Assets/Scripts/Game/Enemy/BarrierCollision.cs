using Game.Player;
using UnityEngine;

namespace Game.Enemy
{
    public class BarrierCollision : MonoBehaviour
    {
        [SerializeField] private int damage;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player")) 
                HitPlayer();
        }

        private void HitPlayer()
        {
            PlayerInfo.Health -= damage;
        }
    }
}