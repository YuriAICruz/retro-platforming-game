using System;
using System.Collections;
using GameManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Graphene.Acting.SideScroller;

namespace Graphene
{
    public class GameData
    {
        public Vector3 waypointPos;
    }

    public class PlatFormerManager : GameManager
    {
        public event Action GameStart;

        private GameData _gameData;

        private static PlatFormerManager _instance;
        private Player _actor;

        public static PlatFormerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var tmp = FindObjectOfType<PlatFormerManager>() ??
                              new GameObject("Manager", new[]
                              {
                                  typeof(PlatFormerManager)
                              }).GetComponent<PlatFormerManager>();

                    _instance = tmp;
                }

                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            _gameData = new GameData();
        }

        public void StartGame()
        {
            Debug.Log("StartGame");
            GameStart?.Invoke();


            StartCoroutine(LocatePlayer());
        }

        IEnumerator LocatePlayer()
        {
            yield return new WaitForSeconds(1.2f);
            
            SceneManager.LoadScene(1);
            
            yield return null;

            while (_actor == null)
            {
                _actor = FindObjectOfType<Player>();
                yield return null;
            }
            
            _actor.OnSetWayPoint += OnSetWayPoint;
        }

        void OnSetWayPoint(Vector3 pos)
        {
            _gameData.waypointPos = pos;
        }
    }
}