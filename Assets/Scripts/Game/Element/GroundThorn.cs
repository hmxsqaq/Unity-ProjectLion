using Game.Player;
using UnityEngine;

namespace Game.Element
{
    public class GroundThorn : MonoBehaviour
    {
        [SerializeField] private int damage;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerInfo.Health -= damage;
            }
        }
    }
}