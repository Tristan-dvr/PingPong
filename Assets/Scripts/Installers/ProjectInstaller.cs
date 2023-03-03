using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IStorage>().To<PlayerPrefsStorage>().AsSingle().NonLazy();

        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<GoalSignal>();
        Container.DeclareSignal<GameLoop.StartMatchSignal>();
        Container.DeclareSignal<GameLoop.MatchDataChangedSignal>();
        Container.DeclareSignal<GamePauseHandler.PauseSignal>();
        Container.DeclareSignal<SceneManagementHandler.LoadSceneSignal>();
        Container.DeclareSignal<RacketHitSignal>();

        Container.BindInterfacesAndSelfTo<SceneManagementHandler>().AsSingle().NonLazy();
    }
}
