using Graphene.UiGenerics;

namespace Graphene.Presentation
{
    public class StartGameSound : AudioSourceView
    {
        private PlatFormerManager _manager;

        private void Setup()
        {
            _manager = PlatFormerManager.Instance;
            _manager.GameStart += Play;
        }
    }
}