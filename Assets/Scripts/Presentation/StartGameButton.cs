using Graphene.UiGenerics;
using UnityEngine;

namespace Graphene.Presentation
{
    public class StartGameButton : ButtonView
    {
        private PlatFormerManager _manager;

        private void Setup()
        {
            _manager = PlatFormerManager.Instance;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Button_Start"))
            {
                _manager.StartGame();
            }
        }

        protected override void OnClick()
        {
            base.OnClick();
            
            Debug.Log("OnClick");
            _manager.StartGame();
        }
    }
}