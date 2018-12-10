using Graphene.UiGenerics;

namespace Graphene.Presentation
{
    public class MainMenu : CanvasGroupView
    {
        private PlatFormerManager _manager;

        private void Setup()
        {
            _manager = PlatFormerManager.Instance;
            _manager.GameStart += Hide;
        }
    }
}