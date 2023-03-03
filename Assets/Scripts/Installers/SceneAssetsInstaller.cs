using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

[CreateAssetMenu(fileName = "SceneAssets", menuName = "PingPong/SceneAssets")]
public class SceneAssetsInstaller : ScriptableObjectInstaller
{
    public AssetReference field;
    public AssetReference playerRacket;
    public AssetReference enemyRacket;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Field>()
            .FromComponentInNewPrefab(GetPrefab(field))
            .AsCached();

        Container.Bind<IRacket>().WithId(PlayerType.Player)
            .To<Racket>()
            .FromComponentInNewPrefab(GetPrefab(playerRacket))
            .AsCached();

        Container.Bind<IRacket>().WithId(PlayerType.Enemy)
            .To<Racket>()
            .FromComponentInNewPrefab(GetPrefab(enemyRacket))
            .AsCached();

        Container.BindIFactory<Ball, BallsFactory>().AsCached();
        Container.BindInterfacesAndSelfTo<Ball>()
            .FromFactory<Ball, BallsFactory>()
            .AsCached();
    }

    private GameObject GetPrefab(AssetReference reference)
    {
        return reference.Asset as GameObject ?? reference.LoadAssetAsync<GameObject>().WaitForCompletion();
    }
}
