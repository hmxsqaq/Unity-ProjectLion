using Game.Player;
using Hmxs.Toolkit.AudioCenter;
using Sirenix.OdinInspector;
using UnityEngine;
using AudioType = Hmxs.Toolkit.AudioCenter.AudioType;

namespace Game.Enemy
{
    public class EnemyLantern : Enemy
    {
        [SerializeField] private int health;
        [SerializeField] private int damage;
        [SerializeField] private float attackCoolDownTime;
        [SerializeField] private float playerSearchDistance;
        [Required]
        [SerializeField] private GameObject bullet;
        [Required]
        [SerializeField] private Transform bulletGeneratePoint;

        private Animator _animator;
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
            _player = GameObject.FindWithTag("Player").transform;
            _attackTimer = Timer.Register(attackCoolDownTime,onComplete: () =>
            {
                Attack();
            }, isLooped: true);
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

        #region Attack

        private void Attack()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect, "灯笼怪攻击音效"));
            _animator.Play("Attack");
        }
        
        private void BulletGenerate()
        { 
            Instantiate(bullet, bulletGeneratePoint.position, Quaternion.identity);
            _animator.Play("Idle");
        }

        #endregion

        #region Trigger
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                HitPlayer();
            else if (other.CompareTag("HitBox")) 
                BeAttacked();
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
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerSearchDistance);
        }
    }
}