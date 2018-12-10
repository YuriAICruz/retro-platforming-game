using UnityEngine;
using Graphene.Acting.Interfaces;

namespace Graphene
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        IProjectile[] _bullets;

        public int BulletCount = 3;
        public float BulletSpeed = 1;

        public Vector3 Tip;
        [SerializeField] private string _bulletResource = "Bullet_A";

        public void Start()
        {
            _bullets = new IProjectile[BulletCount];

            var obj = Resources.Load<Bullet>(_bulletResource);
            for (int i = 0; i < BulletCount; i++)
            {
                var tmp = Instantiate(obj, transform);
                tmp.transform.localPosition = Tip;
                _bullets[i] = tmp;
            }
        }

        public void SetTip(Vector3 shootDirection)
        {
            Tip = shootDirection;
        }

        IProjectile GetNextBullet()
        {
            foreach (var bullet in _bullets)
            {
                if (bullet.Idle)
                    return bullet;
            }
            return null;
        }


        public void Use(Vector3 dir)
        {
            var bl = GetNextBullet();
            if(bl == null) return;

            bl.Shoot(transform.TransformPoint(Tip), dir.normalized*BulletSpeed);
        }
    }
}