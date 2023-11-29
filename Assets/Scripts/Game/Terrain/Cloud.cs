using System;
using UnityEngine;

namespace Game.Terrain
{
    public class Cloud : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float limitLeft;
        [SerializeField] private float limitRight;

        private void Update()
        {
            transform.Translate(Vector3.left * (speed * Time.deltaTime));

            if (transform.position.x < limitLeft)
            {
                var position = transform.position;
                position.Set(limitRight, position.y, position.z);
            }
        }
    }
}