﻿using UnityEngine;
using Graphene.Acting;

namespace Graphene
{
    public class MovingPlatform : MonoBehaviour
    {
        public Vector3 From, To;
        private float _t = 0;
        public float Delay, Speed;
        private float _distance;
        private float _iniTime;

        private void Start()
        {
            From = transform.TransformPoint(From);
            To = transform.TransformPoint(To);
            _distance = (From - To).magnitude;

            _iniTime = Time.time;
        }

        void Update()
        {
            if (Time.time <= _iniTime + Delay) return;
            
            _t += Time.deltaTime;
            transform.position = Vector3.Lerp(From, To, Mathf.Sin(_t * Speed * Mathf.PI) * 0.5f + 0.5f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var actor = other.collider.GetComponent<Actor>();
            if (actor != null && actor.transform.position.y < transform.position.y) return;

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