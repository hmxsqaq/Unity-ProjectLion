using UnityEngine;

namespace Game.Enemy
{
    public class EnemyBarrier : Enemy
    {
        [SerializeField] private int health;
        [SerializeField] private float force;

        protected override void Start()
        {
            Sprite = GetComponent<SpriteRenderer>();
            Collider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                BeAttacked();
                other.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force, ForceMode2D.Impulse);
            }
        }

        private void BeAttacked()
        {
            health -= 1;
            if (health <= 0) Die();
        }
    }
}