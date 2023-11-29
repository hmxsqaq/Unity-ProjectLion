using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/MyPlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Title("Fall")]
        public float fallGravityMult; // 下落重力系数
        public float maxFallSpeed; // 下落最大速度
        public float fastFallGravityMult; // 更大的下落重力系数（当玩家在下落时继续输入向下指令时）
        public float fastMaxFallSpeed; // 更快的下落最大速度（当玩家在下落时继续输入向下指令时）
        
        
        [Title("Run")] 
        public float maxRunSpeed; // 最大速度
        [PropertyRange(0.01f,"maxRunSpeed")] public float runAcceleration; // 加速度
        [PropertyRange(0.01f,"maxRunSpeed")] public float runDeceleration; // 减速度
        [PropertyRange(0f,1f)] public float runAccelerationMultInAir; // 空中加速度系数
        [PropertyRange(0f,1f)] public float runDecelerationMultInAir; // 空中减速度系数
        public bool doConserveMomentum = true; // 是否保持动量
        
        
        [Title("Jump")]
        public float jumpHeight; // 跳跃高度
        public float jumpTime; // 达到跳跃高度所需时间
        public float jumpCutGravityMult; // 当玩家在跳跃过程中松开空格后，所施加的额外重力
        
        [Title("JumpHang")]
        public float jumpHangSpeedThreshold; // 当玩家接近跳跃顶点时会进入“jump hang”状态，增加滞空时间，这是进入这个状态的最大速度
        [PropertyRange(0f,1f)] public float jumpHangGravityMult; // 进入“jump hang”状态的重力系数，应该比正常重力系数小
        public float jumpHangAccelerationMult; // 进入“jump hang”状态的加速度系数
        public float jumpHangMaxSpeedMult; 	// 进入“jump hang”状态的最大速度系数
        
        [Title("JumpAssists")]
        [PropertyRange(0.01f, 0.5f)] public float coyoteTime; // 土狼时间
        [PropertyRange(0.01f, 0.5f)] public float jumpInputBufferTime; // 输入缓冲时间


        [HideInInspector] public float gravityStrength; // 重力强度，与jumpHeight和jumpTime计算得出
        [HideInInspector] public float gravityScale; // 全局重力系数
        
        [HideInInspector] public float actualRunAcceleration; // 实际施加的加速度
        [HideInInspector] public float actualRunDeceleration; // 实际施加的减速度

        [HideInInspector] public float actualJumpForce; // 实际施加的跳跃的力
        
        private void OnValidate()
        {
            gravityStrength = -(2 * jumpHeight) / (jumpTime * jumpTime);
            gravityScale = gravityStrength / Physics2D.gravity.y;

            actualRunAcceleration = 50 * runAcceleration / maxRunSpeed;
            actualRunDeceleration = 50 * runDeceleration / maxRunSpeed;

            actualJumpForce = Mathf.Abs(gravityStrength) * jumpTime;
        }
    }
}