using Game.Player;
using Hmxs.Toolkit.AudioCenter;
using UnityEngine;
using AudioType = Hmxs.Toolkit.AudioCenter.AudioType;

namespace Game.Enemy
{
    public class EnemyFrog : Enemy
    {
        [SerializeField] private int health;
        [SerializeField] private int damage;
        [SerializeField] private float attackCoolDownTime;
        [SerializeField] private float playerSearchDistance;
        [SerializeField] private float jumpForce;

        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private Transform _player;
        private Timer _attackTimer;
        private bool _isFacingRight = true;
        
        #region Check
        
        private bool _isInScope;
        private bool IsInScope
        {
            get => _isInScope;
            set
            {
                if (_isInScope == value)
                    return;
                _isInScope = value;
                OnScopeStateChange();
            }
        }
        
        private void OnScopeStateChange() 
        {
            if (IsInScope) 
                _attackTimer.Resume();
            else 
                _attackTimer.Pause();
        }
        
        #endregion

        protected override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _player = GameObject.FindWithTag("Player").transform;
            _attackTimer = Timer.Register(attackCoolDownTime, Jump, isLooped: true);
            _attackTimer.Pause();
        }

        private void OnDestroy()
        {
            _attackTimer.Cancel();
        }

        private void Update()
        {
            if (IsAlive)
            {
                var playerPosition = _player.position;
                var position = transform.position;
                IsInScope = Vector2.Distance(playerPosition, position) < playerSearchDistance;
                CheckFaceDirection(playerPosition.x > position.x);
            }
        }
        
        #region Move
        
        private void CheckFaceDirection(bool shouldFacingRight)
        {
            if (shouldFacingRight != _isFacingRight)
            {
                _isFacingRight = !_isFacingRight;
                var scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        private void Idle()
        {
            _animator.Play("Idle");
        }
        
        private void Jump()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect, "蛤蟆怪攻击音效"));
            _animator.Play("Jump");
            if (_isFacingRight)
            {
                _rigidbody.AddForce(new Vector2(1,1) * jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                _rigidbody.AddForce(new Vector2(-1,1) * jumpForce, ForceMode2D.Impulse);
            }
        }
        
        #endregion
        
        #region Trigger

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                HitPlayer();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("HitBox"))
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
            if (health <= 0) Died();
        }

        private void Died()
        {
            _rigidbody.simulated = false;
            Die();
        }

        #endregion
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerSearchDistance);
        }
    }
}