using System;
using UnityEngine;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    public class HybridGameInstaller : MonoInstaller
    {
        [Inject] private Settings _settings;

        public override void InstallBindings()
        {
            InstallFactories();
            InstallManagers();
            InstallSignals();
        }

        private void InstallManagers()
        {
            Container.BindInterfacesAndSelfTo<VFXManager>().AsSingle();
        }

        private void InstallFactories()
        {
            Container.BindFactory<Explosion, Explosion.Factory>()
                .FromPoolableMemoryPool<Explosion, ExplosionPool>(poolBinder => poolBinder
                    .WithInitialSize(4)
                    .FromComponentInNewPrefab(_settings.ExplosionPrefab)
                    .UnderTransformGroup("VFX"));
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<StartGameSignal>();
            Container.BindInterfacesAndSelfTo<HybridSignalBus>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public GameObject ExplosionPrefab;
        }

        private class ExplosionPool : MonoPoolableMemoryPool<IMemoryPool, Explosion>
        {
        }
    }
}