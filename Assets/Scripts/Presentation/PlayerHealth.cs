using Graphene.UiGenerics;
using Graphene.Acting.SideScroller;
using UnityEngine;

namespace Graphene.Presentation
{
    public class PlayerHealth : ImageView
    {
        private Player _player;

        private void Setup()
        {
            _player = FindObjectOfType<Player>();
        }

        private void Update()
        {
            Image.fillAmount = _player.Life.Hp / (float)_player.Life.MaxHp;
        }
    }
}