using UnityEngine;
using Graphene.Acting;

namespace Graphene
{
    public class FallingPlatforms : ScreenSensitiveObject
    {
        public float Delay, Speed;
        private float _distance;
        private float _iniTime;

        private bool _fall;
        private Vector3 _iniPos;
        private float _t;
        private Vector3 _iniRot;

        protected override void OnAwake()
        {
            base.OnAwake();

            _iniPos = transform.position;
            _iniRot = transform.eulerAngles;
        }

        protected override void OnUpdate()
        {
            if (!_fall || !_init) return;

            if (Time.time <= _iniTime + Delay)
            {
                _t += Time.deltaTime*10;
                Vibrate();
                return;
            }

            transform.eulerAngles = _iniRot;
            
            transform.Translate(Vector3.down * Speed*Time.deltaTime);
        }

        protected override Bounds GetBounds()
        {
            return new Bounds(_iniPos, Vector3.one);
        }

        private void Vibrate()
        {
            transform.Rotate(0,0,Mathf.Sin(_t*Mathf.PI)*1f, Space.Self);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(_fall) return;
            
            var actor = other.collider.GetComponent<Actor>();
            
            if (actor != null && actor.transform.position.y < transform.position.y) return;

            _iniTime = Time.time;
            _fall = true;
            _t = 0;

            //actor.transform.SetParent(transform);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
        }

        private void OnCollisionExit2D(Collision2D other)
        {
        }

        protected override void Deinit()
        {
            base.Deinit();
            _fall = false;
            
            transform.position = _iniPos;
            transform.eulerAngles = _iniRot;
        }

        protected override void Init()
        {
            base.Init();
            _fall = false;
            
            transform.position = _iniPos;
            transform.eulerAngles = _iniRot;
        }
    }
}