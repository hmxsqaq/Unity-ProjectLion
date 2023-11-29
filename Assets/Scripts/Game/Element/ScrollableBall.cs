using System;
using UnityEngine;

namespace Game.Element
{
    public class ScrollableBall : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private Vector3 _startPos;
        
        private void Start()
        {
            _startPos = transform.position;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void ResetPos()
        {
            transform.position = _startPos;
            _rigidbody.velocity = Vector3.zero;
        }
    }
}