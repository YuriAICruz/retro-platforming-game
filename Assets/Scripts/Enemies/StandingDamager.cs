using UnityEngine;
using  Graphene.Acting.Interfaces;

namespace Graphene.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    public class StandingDamager : MonoBehaviour
    {
        private Collider2D _collider;
        public int Damage;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var dmg = other.GetComponent<IDamageble>();
            dmg.DoDamage(Damage, transform.position);
        }
    }
}