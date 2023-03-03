using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IRacketInput>().WithId(PlayerType.Player)
            .To<PlayerInput>()
            .AsCached();
        Container.BindInterfacesTo<AIInput>().AsCached();
        Container.Bind<IRacketInput>().WithId(PlayerType.Enemy)
            .FromResolve()
            .AsCached();

        Container.BindInterfacesAndSelfTo<SaveGameStateHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<GamePauseHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<MatchStateHandler>().AsSingle();

        Container.BindInterfacesAndSelfTo<GameLoop>().AsSingle();

        Container.Bind<UnityEventsHandler>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
    }
}
