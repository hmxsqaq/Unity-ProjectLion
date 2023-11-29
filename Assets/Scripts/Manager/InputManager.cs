using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public UnityEvent onPause;
        public UnityEvent onUnPause;

        [HideInInspector]
        public Vector2 moveInput;
        public bool JumpWasPressed { get; private set; }
        public bool JumpWasReleased { get; private set; }
        public bool AttackWasPressed { get; private set; }
        public bool InteractWasPressed { get; private set; }

        private bool _pausing;
        private bool _active = true;
        private bool _canAttack;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Debug.LogWarning("InputManager: Multiple Instance");
            
            _pausing = false;
        }

        private void Update()
        {
            if (!_active) return;
            
            if (Input.GetKeyDown(KeyCode.Escape)) 
                InputPause();

            if (_pausing) return;
            
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            JumpWasPressed = Input.GetKeyDown(KeyCode.Space);
            JumpWasReleased = Input.GetKeyUp(KeyCode.Space);
            InteractWasPressed = Input.GetKeyDown(KeyCode.E);
            if (!_canAttack) return;
            AttackWasPressed = Input.GetMouseButtonDown(0);
        }

        private void InputPause()
        {
            if (_pausing)
            {
                _pausing = false;
                Time.timeScale = 1;
                onUnPause?.Invoke();
            }
            else
            {
                _pausing = true;
                Time.timeScale = 0;
                onPause?.Invoke();
            }
        }

        public void SetActive(bool isActive)
        {
            _active = isActive;
            Time.timeScale = isActive ? 1 : 0;
        }

        public void SetCanAttack(bool canAttack) => _canAttack = canAttack;
    }
}