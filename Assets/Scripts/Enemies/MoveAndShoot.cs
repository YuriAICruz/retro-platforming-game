using Graphene.Acting.Interfaces;
using UnityEngine;

namespace Graphene.Enemies
{
    public class MoveAndShoot : ScreenSensitiveObject, IDamageble
    {
        [SerializeField] private string _weaponResource = "Standing_Weapon";

        public float ShootTime = 1;

        public Vector3 From, To;
        public float Delay, Speed;

        private SpriteRenderer[] _renderers;
        private Collider2D _collider;

        private IWeapon _weapon;

        private float _t = 0, _ts = 0;
        private float _iniTime;
        private float _lastDelta;

        public int ContactDamage = 1;

        public Vector2 TipY;
        public bool ChangeTip = true;

        protected override void OnAwake()
        {
            From = transform.TransformPoint(From);
            To = transform.TransformPoint(To);

            var w = Instantiate(Resources.Load<Weapon>(_weaponResource), transform);
            w.transform.localPosition = Vector3.zero;
            _weapon = w;

            _weapon.SetTip(TipY);

            _renderers = GetComponentsInChildren<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        protected override void OnUpdate()
        {
            if (Time.time <= _iniTime + Delay) return;

            if (!_init) return;

            _t += Time.deltaTime;
            var delta = Mathf.Sin(_t * Speed * Mathf.PI) * 0.5f + 0.5f;
            transform.position = Vector3.Lerp(From, To, delta);

            _ts += Time.deltaTime;

            if (_ts >= ShootTime)
            {
                _ts = 0;
                var dir = Mathf.Sign(-delta + _lastDelta);

                if (ChangeTip)
                {
                    _weapon.SetTip(new Vector3(TipY.x * dir, TipY.y));
                    _weapon.Use(new Vector2(dir, 0));
                    
                    foreach (var spriteRenderer in _renderers)
                    {
                        spriteRenderer.flipX = dir > 0;
                    }
                }
                else
                {
                    _weapon.Use(new Vector2(TipY.x, 0));
                }
            }
            _lastDelta = delta;
        }

        protected override void Deinit()
        {
            base.Deinit();
            foreach (var rend in _renderers)
            {
                rend.enabled = false;
            }
            _collider.enabled = false;
        }

        protected override void Init()
        {
            base.Init();
            foreach (var rend in _renderers)
            {
                rend.enabled = true;
            }
            _t = 0;
            _ts = 0;
            _collider.enabled = true;
            _iniTime = Time.time;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var actor = other.collider.GetComponent<IDamageble>();
            if (actor == null) return;

            actor.DoDamage(ContactDamage, transform.position);
        }

        public void DoDamage(int damage, Vector3 from)
        {
            Life.ReceiveDamage(damage);
        }
    }
}