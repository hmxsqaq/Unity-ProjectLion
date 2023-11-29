using System;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.CameraControl
{
    public class PlayerFollow : MonoBehaviour
    {
        public float speedTime;
        public float distanceLimit;
        public float yFix;
        
        private Transform _player;

        [SerializeField] private Vector2 xLimit = new(-2f , 5f);
        [SerializeField] private Vector2 yLimit = new(-1f, 2f);
        
        private Vector3 _pos;
        private Vector3 _playerPos;
        private Vector2 _v;

        public static PlayerFollow Instance;
        
        private void Start()
        {
            if (Instance == null) Instance = this;
            else Debug.LogWarning("PlayerFollow: Multiple Instance");
            
            _player = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            SmoothDampFollow();
        }

        private void SmoothDampFollow()
        {
            _pos = transform.position;
            _playerPos = _player.position + new Vector3(0, yFix, 0);
            if (Vector2.Distance(_pos, _player.position) > distanceLimit)
                _pos = Vector2.SmoothDamp(_pos, _playerPos, ref _v , speedTime);
            else
                _pos = _playerPos;
            _pos.z = -10;
            _pos.x = Mathf.Clamp(_pos.x, xLimit.x, xLimit.y);
            _pos.y = Mathf.Clamp(_pos.y, yLimit.x, yLimit.y);
            transform.position = _pos;
        }

        public void SetClamp(Vector2 newXLimit = default, Vector2 newYLimit = default)
        {
            if (newXLimit != default) xLimit = newXLimit;
            if (newYLimit != default) xLimit = newXLimit;
        }
    }
}