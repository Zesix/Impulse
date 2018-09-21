using Zenject;

namespace Impulse
{
    public class SaveControllerInstaller : MonoInstaller<SaveControllerInstaller>
    {
        public override void InstallBindings()
        {
            transform.GetComponent<GameObjectContext>().Container.BindInstance(GetComponent<ILocalPlayerProfileService>());
        }
    }
}
