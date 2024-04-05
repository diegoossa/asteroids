using UnityEngine;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    /// <summary>
    /// Installer for the hybrid settings.
    /// </summary>
    [CreateAssetMenu(fileName = "HybridSettingsInstaller", menuName = "Installers/HybridSettingsInstaller")]
    public class HybridSettingsInstaller : ScriptableObjectInstaller<HybridSettingsInstaller>
    {
        public HybridGameInstaller.Settings GameInstallerSettings;
        
        public override void InstallBindings()
        {
            Container.BindInstance(GameInstallerSettings).IfNotBound();
        }
    }
}