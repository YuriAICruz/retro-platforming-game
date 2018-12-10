using Graphene.Acting.SideScroller;
using UnityEditor;
using UnityEngine;

namespace Graphene
{
    public class PopOntrigger : ScreenSensitiveObject
    {
        private Collider2D _collider;
        private SpriteRenderer _renderer;

        protected override void OnAwake()
        {
            base.OnAwake();
            
            _collider = GetComponent<Collider2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var pl = other.GetComponent<Player>();
            if(pl  == null) return;

            Show();
        }

        private void Show()
        {
            _collider.enabled = true;
            _renderer.enabled = true;
        }

        protected override Bounds GetBounds()
        {
            return new Bounds(transform.position, Vector3.one* 4);
        }

        protected override void Deinit()
        {
            base.Deinit();
            _collider.enabled = false;
            _renderer.enabled = false;
        }
    }
}