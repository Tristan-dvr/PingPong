using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "BallsCustomization", menuName = "PingPong/Balls customization")]
public class BallCustomizationInstaller : ScriptableObjectInstaller
{
    public CustomBalls customBalls = new CustomBalls();

    public override void InstallBindings()
    {
        Container.BindInstance(customBalls).AsSingle();
    }
}
