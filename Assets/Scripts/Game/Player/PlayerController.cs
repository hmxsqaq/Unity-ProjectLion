using System;
using Hmxs.Toolkit.AudioCenter;
using Hmxs.Toolkit.Events;
using Manager;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.Events;
using AudioType = Hmxs.Toolkit.AudioCenter.AudioType;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Required] public PlayerData data;

        [Title("Checks")]
        [SerializeField] private Transform groundCheckPoint; // 地面检测点
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f); // 地面检测范围
        [SerializeField] private LayerMask groundLayer; // 地面Layer
        
        [Title("Attack")]
        [SerializeField] private GameObject hitBox; // 攻击检测盒

        [Title("Revive")] 
        [SerializeField] private Transform checkPoint; // 重生点
        [SerializeField] private UnityEvent onPlayerDie;

        #region Field and Property

        // Component
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        
        // State
        private bool _isFacingRight;
        private bool _isJumping;
        private bool _isOnGround;
        private bool _isJumpCut;
        private bool _isJumpFalling;
        private bool _isAttacking;
        
        // Timer
        private float _lastOnGroundTime;
        private float _lastPressedJumpTime;
        
        // Run
        private Vector2 _moveInput;

        // AnimSate
        private static readonly int OnGround = Animator.StringToHash("OnGround");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int Moving = Animator.StringToHash("Moving");

        #endregion

        #region Public Method

        public void SetCheckPoint(Transform newCheckPoint)
        {
            checkPoint = newCheckPoint;
        }

        public Transform GetCheckPoint() => checkPoint;

        #endregion
        
        #region UnityFunction
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            PlayerInfo.OnHealthChange += BeHit;
        }

        private void OnDestroy()
        {
            PlayerInfo.OnHealthChange -= BeHit;
        }

        private void Start()
        {
            AudioCenter.Instance.SetVolume(AudioType.Effect, 0.5f);
            SetGravityScale(data.gravityScale);
            _isFacingRight = true;
        }

        private void Update()
        {
            TimerUpdate();
            InputHandler();
            JumpCheck();
            GravityApply();
        }

        private void FixedUpdate()
        {
            CollisionCheck();
            Run();
            AnimatorControl();
        }
        
        #endregion

        #region General Method

        private void SetGravityScale(float scale) => _rigidbody.gravityScale = scale;

        #endregion
        
        #region Timer
        
        // 每帧更新各计时器
        private void TimerUpdate()
        {
            _lastOnGroundTime -= Time.deltaTime;
            _lastPressedJumpTime -= Time.deltaTime;
        }
        
        #endregion
        
        #region InputHandler
        
        // 输入处理
        private void InputHandler()
        {
            _moveInput = InputManager.Instance.moveInput;

            if (_moveInput.x != 0) 
                CheckFaceDirection(_moveInput.x > 0);

            if (InputManager.Instance.JumpWasPressed) 
                OnJumpPressed();

            if (InputManager.Instance.JumpWasReleased)
                OnJumpReleased();

            if (InputManager.Instance.InteractWasPressed) 
                EventCenter.Trigger(EventName.Player.SubmitPressed);

            if (InputManager.Instance.AttackWasPressed) 
                AttackApply();
        }
        
        #endregion

        #region CollisionCheck

        private void CollisionCheck()
        {
            if (!_isJumping)
            {
                GroundCheck();
            }
        }

        private void GroundCheck()
        {
            _isOnGround = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
            if (_isOnGround) _lastOnGroundTime = data.coyoteTime;
        }

        // private void OnCollisionStay2D(Collision2D other)
        // {
        //     if (other.gameObject.CompareTag("Circle"))
        //     {
        //         Vector2 point = other.GetContact(0).point;
        //         Vector2 direction = (point - (Vector2)transform.position).normalized;
        //         direction = Vector2.Perpendicular(direction);
        //         _rigidbody.AddForce(direction, ForceMode2D.Impulse);
        //     }
        // }

        #endregion
        
        #region Turn

        // 检查角色面向方位
        private void CheckFaceDirection(bool isMovingRight)
        {
            if (isMovingRight != _isFacingRight) Turn();
        }
        
        // 转向
        private void Turn()
        {
            _isFacingRight = !_isFacingRight;
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        #endregion

        #region Jump

        // jump键被按下时
        private void OnJumpPressed() => _lastPressedJumpTime = data.jumpInputBufferTime;

        // jump键被松开时
        private void OnJumpReleased()
        {
            // 如果正在跳跃上升过程中松开空格，则进入Jump截断状态
            if (_isJumping && _rigidbody.velocity.y > 0) 
                _isJumpCut = true;
        }

        // 检查Jump相关状态
        private void JumpCheck()
        {
            // 正在跳跃且y轴速度为负，则开始下落
            if (_isJumping && _rigidbody.velocity.y < 0)
            {
                _isJumping = false;
                _isJumpFalling = true;
            }
            
            // 在地面上且没在跳，则取消Jump的截断与下落状态,此时可以跳跃
            if (_lastOnGroundTime > 0 && !_isJumping)
            {
                _isJumpCut = false;
                _isJumpFalling = false;
                if (_lastPressedJumpTime > 0)
                {
                    _isJumping = true;
                    Jump();
                }
            }
            
            // 若按下Jump键的时间在Buffer时间内，则跳跃
            // if (_lastOnGroundTime > 0 && !_isJumping && _lastPressedJumpTime > 0)
            // {
            //     _isJumping = true;
            //     _isJumpCut = false;
            //     _isJumpFalling = false;
            //     Jump();
            // }
        }
        
        private void Jump()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect, "跳跃音效"));
            _lastOnGroundTime = 0;
            _lastPressedJumpTime = 0;
            var force = data.actualJumpForce;
            if (_rigidbody.velocity.y < 0) // 我们在Buffer时间中跳跃的话，跳跃的时刻我们正在下落，需要施加更大的力
                force -= _rigidbody.velocity.y;
            _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        #endregion

        #region Gravity

        private void GravityApply()
        {
            var velocity = _rigidbody.velocity;
            if (velocity.y < 0 && _moveInput.y < 0)
            {
                // 下落时按s会获得更高的重力
                SetGravityScale(data.gravityScale * data.fastFallGravityMult);
                // 对最大下落速度进行限制
                _rigidbody.velocity = new Vector2(velocity.x, Mathf.Max(velocity.y, -data.fastMaxFallSpeed));
            }
            else if (_isJumpCut)
            {
                // Jump截断时
                SetGravityScale(data.gravityScale * data.jumpCutGravityMult);
                // 对最大下落速度进行限制
                _rigidbody.velocity = new Vector2(velocity.x, Mathf.Max(velocity.y, -data.maxFallSpeed));
            }
            else if ((_isJumping || _isJumpFalling) && Mathf.Abs(velocity.y) < data.jumpHangSpeedThreshold)
            {
                // 如果在跳或者在下落，并且y轴速度小于Hang状态临界值，则进入JumpHang状态，受到更小的重力
                SetGravityScale(data.gravityScale * data.jumpHangGravityMult);
            }
            else if (velocity.y < 0)
            {
                // 下落时受到额外的重力
                SetGravityScale(data.gravityScale * data.fallGravityMult);
                // 对最大下落速度进行限制
                _rigidbody.velocity = new Vector2(velocity.x, Mathf.Max(velocity.y, -data.maxFallSpeed));
            }
            else
            {
                // 默认状态，默认重力
                SetGravityScale(data.gravityScale);
            }
        }

        #endregion

        #region Run

        private void Run()
        {
            var targetSpeed = _moveInput.x * data.maxRunSpeed; // 目标速度
            targetSpeed = Mathf.Lerp(_rigidbody.velocity.x, targetSpeed, 1);

            float accelerationRate; // 加速度系数
            if (_lastOnGroundTime > 0)
            {
                // 在地面上
                accelerationRate = Mathf.Abs(targetSpeed) > 0.01f
                    ? data.actualRunAcceleration
                    : data.actualRunDeceleration;
            }
            else
            {
                // 在空中
                accelerationRate = Mathf.Abs(targetSpeed) > 0.01f
                    ? data.actualRunAcceleration * data.runAccelerationMultInAir
                    : data.actualRunDeceleration * data.runDecelerationMultInAir;
            }

            if ((_isJumping || _isJumpFalling) && Mathf.Abs(_rigidbody.velocity.y) < data.jumpHangSpeedThreshold)
            {
                // 如果在跳或者在下落，并且y轴速度小于Hang状态临界值，则进入JumpHang状态,增加最大速度/加速度
                targetSpeed *= data.jumpHangMaxSpeedMult;
                accelerationRate *= data.jumpHangAccelerationMult;
            }


            if (data.doConserveMomentum &&
                Mathf.Abs(_rigidbody.velocity.x) > Mathf.Abs(targetSpeed) &&
                Math.Abs(Mathf.Sign(_rigidbody.velocity.x) - Mathf.Sign(targetSpeed)) == 0 &&
                Mathf.Abs(targetSpeed) > 0.01f && _lastOnGroundTime < 0)
            {
                // 动量维持
                accelerationRate = 0;
            }

            var speedDifference = targetSpeed - _rigidbody.velocity.x;
            var actualForce = speedDifference * accelerationRate;
            _rigidbody.AddForce(actualForce * Vector2.right, ForceMode2D.Force);
        }
        #endregion

        #region Animator

        private void AnimatorControl()
        {
            _animator.SetBool(OnGround, _isOnGround);
            _animator.SetBool(Jumping, _isJumping);
            _animator.SetBool(Moving, _moveInput.x != 0);
        }

        #endregion

        #region Attack

        private void AttackApply()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect, "歌沂攻击音效"));
            _animator.Play("Attack");
        }
        
        private void AttackStart()
        {
            hitBox.SetActive(true);
        }

        private void AttackEnd()
        {
            hitBox.SetActive(false);
            _animator.Play("Idle");
        }

        #endregion
        
        #region Hited

        private void BeHit()
        {
            if (PlayerInfo.Health < 1) 
                Die();
        }

        private void Die()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect, "歌沂阵亡音效"));
            _animator.Play("Die");
            FadeEffect.Instance.Fade(1f,Color.clear, Color.black, 
                onStart: () => InputManager.Instance.SetActive(false),
                onFinish: () =>
                {
                    FadeEffect.Instance.Fade(1f, Color.black, Color.clear,
                        onStart: () =>
                        {
                            transform.position = checkPoint.position;
                            InputManager.Instance.SetActive(true);
                            _animator.Play("Idle");
                            onPlayerDie?.Invoke();
                        });
                });
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
        }

        #endregion
    }
}