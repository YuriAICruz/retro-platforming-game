using UnityEngine;
using Graphene.Acting;

namespace Graphene
{
    public class FallingPlatforms : ScreenSensitiveObject
    {
        public float Delay, Speed;
        private float _distance;
        private float _iniTime;


        protected override void OnUpdate()
        {
            if (!_init || Time.time <= _iniTime + Delay) return;
            
            transform.Translate(Vector3.down * Speed*Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var actor = other.collider.GetComponent<Actor>();
            
            if (actor != null && actor.transform.position.y < transform.position.y) return;

            _iniTime = Time.time;

            actor.transform.SetParent(transform);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var actor = other.collider.GetComponent<Actor>();
            
            if (actor == null) return;

            actor.transform.SetParent(null);
        }
    }
}