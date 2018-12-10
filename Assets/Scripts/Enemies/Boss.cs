using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Graphene.Acting;
using Graphene.BehaviourTree;
using Graphene.BehaviourTree.Actions;
using Graphene.BehaviourTree.Composites;
using Graphene.BehaviourTree.Conditions;
using UnityEngine;
using Behaviour = Graphene.BehaviourTree.Behaviour;
using Graphene.Acting.Interfaces;
using Graphene.Acting.SideScroller;
using Graphene.BehaviourTree.Decorators;

namespace Graphene.Enemies
{
    enum BlackboardIds
    {
        RuningRoutine = 1,
        RunRoutineA = 2,
        RunRoutineB = 3,
        PlayerClose = 4
    }

    public class Boss : MonoBehaviour, IDamageble
    {
        public Life Life;

        private Camera _cam;
        private Plane[] _planes;

        protected bool _init;

        private IWeapon _weapon;
        [SerializeField] private string _weaponResource = "Standing_Weapon";

        private bool _dead;

        private Behaviour _tree;
        private Blackboard _blackboard;
        private Player _player;

        public Transform[] Anchors;

        public int ContactDamage = 2;
        public float PlayerDistance;
        public float Speed = 1;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            _cam = Camera.main;
            _planes = GeometryUtility.CalculateFrustumPlanes(_cam);

            Life.Reset();
            Life.OnDie += OnDie;

            var w = Instantiate(Resources.Load<Weapon>(_weaponResource), transform);
            w.transform.localPosition = Vector3.zero;
            _weapon = w;

            _blackboard = new Blackboard();
            _tree = new Behaviour();

            _player = FindObjectOfType<Player>();

            SetupDelegates();

            _tree.root = new Priority(
                new List<Node>
                {
                    new Sequence(new List<Node>()
                    {
                        new CallSystemActionMemory((int) BlackboardIds.PlayerClose),
                        new DebugLog("Close Attack"),
                        new MemorySequence(new List<Node>()
                        {
                            new CallSystemActionMemory((int) BlackboardIds.RunRoutineA),
                        })
                    }),
                    new Sequence(new List<Node>()
                    {
                        new Inverter(new List<Node>()
                        {
                            new CallSystemActionMemory((int) BlackboardIds.PlayerClose),
                        }),
                        new DebugLog("Far Attack"),
                        new MemorySequence(new List<Node>()
                        {
                            new CallSystemActionMemory((int) BlackboardIds.RunRoutineB),
                        })
                    }),
                    new ChangeColor(Color.blue),
                });
        }

        private void SetupDelegates()
        {
            _blackboard.Set((int) BlackboardIds.RuningRoutine, false, _tree.id);
            _blackboard.Set((int) BlackboardIds.RunRoutineA, new Behaviour.NodeResponseAction(RunningRoutineA), _tree.id);
            _blackboard.Set((int) BlackboardIds.RunRoutineB, new Behaviour.NodeResponseAction(RunningRoutineB), _tree.id);

            _blackboard.Set((int) BlackboardIds.PlayerClose, new Behaviour.NodeResponseAction(PlayerClose), _tree.id);
        }

        void Shoot()
        {
            _animator.SetTrigger("Shoot");
            var dir = _player.transform.position - transform.position;
            _weapon.Use(dir.normalized);
        }

        bool _runningRoutine;

        private NodeStates RunningRoutineA()
        {
            _blackboard.Set((int) BlackboardIds.RuningRoutine, _runningRoutine, _tree.id);
            StartCoroutine(RoutineA());

            return _runningRoutine ? NodeStates.Running : NodeStates.Success;
        }

        IEnumerator RoutineA()
        {
            _runningRoutine = true;

            var right = Random.Range(0, 10) >= 5;

            var t = 0f;
            var p = transform.position;
            
            _animator.SetBool("Jumping", true);
            while (t < 1)
            {
                t += Time.deltaTime * Speed;

                transform.position = Vector3.Lerp(p, Anchors[right ? 1 : 3].position, t);

                yield return null;
            }

            _animator.SetBool("Jumping", false);
            yield return null;

            Shoot();

            yield return new WaitForSeconds(0.3f);

            Shoot();

            yield return new WaitForSeconds(0.15f);

            Shoot();

            yield return new WaitForSeconds(0.1f);

            t = 0f;
            p = transform.position;
            _animator.SetBool("Jumping", true);
            while (t < 1)
            {
                t += Time.deltaTime * Speed;

                transform.position = Vector3.Lerp(p, Anchors[right ? 4 : 5].position, t);

                yield return null;
            }
            _animator.SetBool("Jumping", false);

            _runningRoutine = false;
        }


        private NodeStates RunningRoutineB()
        {
            _blackboard.Set((int) BlackboardIds.RuningRoutine, _runningRoutine, _tree.id);
            StartCoroutine(RoutineB());

            return _runningRoutine ? NodeStates.Running : NodeStates.Success;
        }

        IEnumerator RoutineB()
        {
            _runningRoutine = true;

            var right = Random.Range(0, 10) >= 5;

            var t = 0f;
            var p = transform.position;
            _animator.SetBool("Jumping", true);
            while (t < 1)
            {
                t += Time.deltaTime * Speed;

                transform.position = Vector3.Lerp(p, Anchors[0].position, t);

                yield return null;
            }
            _animator.SetBool("Jumping", false);

            for (var i = 0; i < 6; i++)
            {
                Shoot();

                yield return new WaitForSeconds(0.3f);
            }

            t = 0f;
            p = transform.position;
            
            while (t < 1)
            {
                t += Time.deltaTime * Speed;

                transform.position = Vector3.Lerp(p, Anchors[right ? 4 : 5].position, t);

                yield return null;
            }

            _runningRoutine = false;
        }

        private NodeStates PlayerClose()
        {
            var dir = _player.transform.position - transform.position;

            return dir.magnitude < PlayerDistance ? NodeStates.Failure : NodeStates.Success;
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

            if (!_init) return;

            _tree.Tick(this.gameObject, _blackboard);
        }


        private void OnDie()
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
            _animator.SetTrigger("init");
        }

        public void DoDamage(int damage, Vector3 from)
        {
            _animator.SetTrigger("Hit");
            Life.ReceiveDamage(damage);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var actor = other.collider.GetComponent<IDamageble>();
            if (actor == null) return;

            actor.DoDamage(ContactDamage, transform.position);
        }
    }
}