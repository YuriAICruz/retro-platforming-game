using UnityEngine;
using Graphene.Acting.Interfaces;

namespace Graphene.Enemies
{
    public class StandingTurret : ScreenSensitiveObject, IDamageble
    {
        private IWeapon _weapon;
        

        private Renderer[] _renderers;
        private Collider2D _collider;
        

        public float ShootTime = 1, Delay;

        public Vector2 ShootDirection;

        [SerializeField] private string _weaponResource = "Standing_Weapon";

        float _t = 0;
        protected float _iniTime;

        protected override void OnAwake()
        {
            var w = Instantiate(Resources.Load<Weapon>(_weaponResource), transform);
            w.transform.localPosition = Vector3.zero;
            _weapon = w;

            _weapon.SetTip(new Vector3(ShootDirection.x, ShootDirection.y));
            
            _renderers = GetComponentsInChildren<Renderer>();
            _collider = GetComponent<Collider2D>();
        }

        protected override void OnUpdate()
        {
            if (Time.time <= _iniTime + Delay) return;

            if (!_init) return;

            _t += Time.deltaTime;

            if (_t >= ShootTime)
            {
                _t = 0;
                _weapon.Use(ShootDirection);
            }
        }

        public void DoDamage(int damage, Vector3 from)
        {
            Life.ReceiveDamage(damage);
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
            _collider.enabled = true;
            _iniTime = Time.time;
        }
    }
}