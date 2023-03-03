using System.Linq;
using UnityEngine;
using Zenject;

public class BallsFactory : IFactory<Ball>
{
    private CustomBalls _customBalls;
    private IStorage _storage;
    private DiContainer _container;

    public BallsFactory(CustomBalls customBalls, IStorage storage, DiContainer container)
    {
        _customBalls = customBalls;
        _storage = storage;
        _container = container;
    }

    public Ball Create()
    {
        var data = _storage.GetState();
        var ballConfig = _customBalls.balls.FirstOrDefault(b => b.id == data.selectedBallId);
        if (ballConfig == null)
            ballConfig = _customBalls.defaultBall;

        var prefab = ballConfig.prefab.Asset as GameObject ?? ballConfig.prefab.LoadAssetAsync<GameObject>().WaitForCompletion();
        return _container.InstantiatePrefabForComponent<Ball>(prefab);
    }
}
