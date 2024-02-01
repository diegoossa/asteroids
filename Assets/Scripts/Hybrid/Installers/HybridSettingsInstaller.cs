using UnityEngine;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    [CreateAssetMenu(fileName = "HybridSettingsInstaller", menuName = "Installers/HybridSettingsInstaller")]
    public class HybridSettingsInstaller : ScriptableObjectInstaller<HybridSettingsInstaller>
    {
        public HybridGameInstaller.Settings GameInstallerSettings;
        public AudioHandler.Settings AudioHandler;
        
        public override void InstallBindings()
        {
            Container.BindInstance(GameInstallerSettings).IfNotBound();
            Container.BindInstance(AudioHandler);
        }
    }
}