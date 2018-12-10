using UnityEngine;
using Graphene.Acting;

namespace Graphene
{
    public class ScreenSensitiveObject : MonoBehaviour
    {
        public Life Life;

        private Camera _cam;
        private Plane[] _planes;

        protected bool _init;

        private bool _dead;

        private void Awake()
        {
            _cam = Camera.main;
            _planes = GeometryUtility.CalculateFrustumPlanes(_cam);

            Life.Reset();
            Life.OnDie += OnDie;

            OnAwake();
        }

        private void Update()
        {
            GeometryUtility.CalculateFrustumPlanes(_cam, _planes);
            var res = GeometryUtility.TestPlanesAABB(_planes, new Bounds(transform.position, Vector3.one));

            if (!res)
            {
                _dead = false;
                if (_init)
                    Deinit();
            }
            else if (!_init && res && !_dead)
            {
                Init();
            }

            OnUpdate();
        }

        void OnDie()
        {
            Deinit();
            _dead = true;
        }

        protected virtual void Deinit()
        {
            _init = false;
        }

        protected virtual void Init()
        {
            _init = true;
        }


        protected virtual void OnAwake()
        {
        }

        protected virtual void OnUpdate()
        {
        }
    }
}