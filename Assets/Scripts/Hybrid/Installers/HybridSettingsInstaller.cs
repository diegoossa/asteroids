using UnityEngine;
using Zenject;

namespace DO.Asteroids.Hybrid
{
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