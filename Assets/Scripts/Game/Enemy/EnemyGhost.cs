using System;
using Game.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyGhost : Enemy
    {
        [SerializeField] private int health;
        [SerializeField] private int damage;
        [SerializeField] private float speed;
        [Required]
        [SerializeField] private Transform[] movePoint;
        
        private int _currentTargetPointIndex;
        private Vector3 _direction;
        private bool _isFacingRight = true;

        private void Update()
        {
            if (IsAlive)
            {
                Move();
                CheckFaceDirection(_direction.x > 0);
            }
        }

        #region Move
        
        private void Move()
        {
            _direction = (movePoint[_currentTargetPointIndex].position - transform.position).normalized;
            transform.Translate(_direction * (Time.deltaTime * speed));
            if (Vector3.Distance(transform.position, movePoint[_currentTargetPointIndex].position) < 0.05f)
                _currentTargetPointIndex =
                    _currentTargetPointIndex == movePoint.Length - 1 ? 0 : _currentTargetPointIndex + 1;
        }

        private void CheckFaceDirection(bool isMovingRight)
        {
            if (isMovingRight != _isFacingRight)
            {
                _isFacingRight = !_isFacingRight;
                var scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
        
        #endregion

        #region Trigger
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                HitPlayer();
            }
            else if (other.CompareTag("HitBox"))
            {
                BeAttacked();
            }
        }

        private void HitPlayer()
        {
            PlayerInfo.Health -= damage;
        }

        private void BeAttacked()
        {
            health -= 1;
            if (health <= 0) Die();
        }

        #endregion
    }
}