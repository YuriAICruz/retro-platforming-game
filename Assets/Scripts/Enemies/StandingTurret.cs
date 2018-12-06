using System.Runtime.InteropServices;
using UnityEngine;
using Graphene.Acting.Interfaces;

namespace Graphene.Enemies
{
    public class StandingTurret : MonoBehaviour, IDamageble
    {
        private IWeapon _weapon;

        public float ShootTime = 1;

        public Vector2 ShootDirection;
        
        [SerializeField] private string _weaponResource = "Standing_Weapon";

        float _t = 0;

        private void Awake()
        {
            var w = Instantiate(Resources.Load<Weapon>(_weaponResource), transform);
            w.transform.localPosition = Vector3.zero;
            _weapon = w;
        }

        private void Update()
        {
            _t += Time.deltaTime;

            if (_t >= ShootTime)
            {
                _t = 0;
                _weapon.Use(ShootDirection);
            }
        }

        public void DoDamage(int damage, Vector3 from)
        {
            Debug.Log($"DoDamage: {damage} - {gameObject}");
        }
    }
}