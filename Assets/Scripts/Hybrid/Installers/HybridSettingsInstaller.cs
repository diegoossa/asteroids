using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "HybridSettingsInstaller", menuName = "Installers/HybridSettingsInstaller")]
public class HybridSettingsInstaller : ScriptableObjectInstaller<HybridSettingsInstaller>
{
    public override void InstallBindings()
    {
    }
}