using UnityEngine;
using Graphene.Acting;

namespace Graphene
{
    public class HidingPlatforms : MonoBehaviour
    {
        public float Delay, TimeShown, TimeHided;

        private float _t = 0;
        private float _iniTime;

        public bool Hided;
        private Collider2D _collider;
        private Actor _actor;
        private SpriteRenderer _renderer;

        private void Start()
        {
            _iniTime = Time.time;

            _collider = GetComponent<Collider2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (Time.time <= _iniTime + Delay) return;

            _t += Time.deltaTime;
            if (Hided && _t > TimeHided)
            {
                Show();
                _t = 0;
            }
            else if (!Hided && _t > TimeShown)
            {
                Hide();
                _t = 0;
            }
        }

        void Hide()
        {
            Hided = true;
//            
//            if(_actor!=null)
//                _actor.transform.SetParent(transform);

            _collider.enabled = false;
            _renderer.enabled = false;
        }

        void Show()
        {
            Hided = false;
            
            _collider.enabled = true;
            _renderer.enabled = true;
        }

//        private void OnCollisionEnter2D(Collision2D other)
//        {
//            var actor = other.collider.GetComponent<Actor>();
//            if (actor != null && actor.transform.position.y < transform.position.y) return;
//
//            _actor = actor;
//            actor.transform.SetParent(transform);
//        }
//
//        private void OnCollisionStay2D(Collision2D other)
//        {
//        }
//
//        private void OnCollisionExit2D(Collision2D other)
//        {
//            var actor = other.collider.GetComponent<Actor>();
//            if (actor == null) return;
//            
//            _actor = null;
//            actor.transform.SetParent(null);
//        }
    }
}