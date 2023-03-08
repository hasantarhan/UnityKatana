using System;
using UnityEngine;

namespace _Game.Code.Utils
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            transform.position = target.position.WithY(transform.position.y);
        }
    }
}