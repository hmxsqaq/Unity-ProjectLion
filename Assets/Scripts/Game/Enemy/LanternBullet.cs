using Game.Player;
using UnityEngine;

namespace Game.Enemy
{
    public class LanternBullet : MonoBehaviour
    {
        [SerializeField] private float flyMaxDistance;
        [SerializeField] private float speed;
        [SerializeField] private int damage;
        
        private Vector2 _startPoint;
        private Vector2 _direction;

        private void Start()
        {
            var model = transform.GetChild(0);
            var player = GameObject.FindWithTag("Player").transform;
            _startPoint = transform.position;
            _direction = ((Vector2)player.position - _startPoint).normalized;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Deg2Rad;
            model.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if (model.position.x < player.position.x) 
                model.GetComponent<SpriteRenderer>().flipX = true;
        }

        private void Update()
        {
            transform.Translate(_direction * (Time.deltaTime * speed));
            if (Vector2.Distance(_startPoint, transform.position) > flyMaxDistance) 
                Die();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                return;
            if (other.CompareTag("Player")) 
                HitPlayer();
            Die();
        }
        
        private void HitPlayer()
        {
            PlayerInfo.Health -= damage;
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, flyMaxDistance);
        }
    }
}