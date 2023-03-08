using System;
using System.Collections;
using UnityEngine;

namespace _Game.Code
{
    [RequireComponent(typeof(Animator))]
    public class AutoAnimator : MonoBehaviour
    {
        private Animator animator;
        private VelocityUtil velocityUtil;
        private float maxSpeed=4;
        public float speed;
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            velocityUtil = new VelocityUtil(transform);
        }

        private void Update()
        {
            velocityUtil.Update();
            speed = velocityUtil.speed;
            speed = speed.Remap(0, maxSpeed, 0, 1);
            animator.SetFloat("Speed", speed);
        }
    }
}