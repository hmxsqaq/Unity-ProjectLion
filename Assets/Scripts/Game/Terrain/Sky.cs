using System;
using UnityEngine;

namespace Game.Terrain
{
    public class Sky : MonoBehaviour
    {
        private Transform _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            var position = transform.position;
            position.Set(_player.position.x, position.y, position.z);
            transform.position = position;
        }
    }
}