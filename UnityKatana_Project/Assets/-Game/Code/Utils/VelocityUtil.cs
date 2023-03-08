using UnityEngine;


    public class VelocityUtil
    {
        private readonly Transform _transform;
        private Vector3 lastPosition;
        public Vector3 Motion { get; private set; }

        public float speed;

        private bool firstWork;
        public VelocityUtil(Transform transform)
        {
            _transform = transform;
        }


        public void Update()
        {
            if (!firstWork)
            {
                lastPosition = _transform.position;
                firstWork = true;
            }
            var position = _transform.position;
            Motion = (position - lastPosition) / Time.deltaTime;
            speed = Motion.magnitude;
            lastPosition = position;
            
        }
    }
