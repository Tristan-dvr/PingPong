using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Settings", menuName = "PingPong/Settings")]
public class SettingsInstaller : ScriptableObjectInstaller
{
    public LayerMask ballCollisionMask;
    public GameSettings gameSettings;

    public override void InstallBindings()
    {
        Container.BindInstances(ballCollisionMask, gameSettings);
    }
}
