using Graphene.UiGenerics;

namespace Graphene.Presentation
{
    public class MainMenuButton : ButtonView
    {
        private MainMenu _menu;

        private void Setup()
        {
            _menu = FindObjectOfType<MainMenu>();
        }

        protected override void OnClick()
        {
            base.OnClick();
            
            _menu.Show();
        }
    }
}