using System;
using UnityEngine;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    /// <summary>
    /// Handles the installation of the hybrid components of the game.
    /// </summary>
    public class HybridGameInstaller : MonoInstaller
    {
        [Inject] private Settings _settings;

        public override void InstallBindings()
        {
            InstallFactories();
            InstallManagers();
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
            
            Container.BindFactory<SpawnVFX, SpawnVFX.Factory>()
                .FromPoolableMemoryPool<SpawnVFX, SpawnVFXPool>(poolBinder => poolBinder
                    .WithInitialSize(1)
                    .FromComponentInNewPrefab(_settings.SpawnVFXPrefab)
                    .UnderTransformGroup("VFX"));
        }

        [Serializable]
        public class Settings
        {
            public GameObject ExplosionPrefab;
            public GameObject SpawnVFXPrefab;
        }

        private class ExplosionPool : MonoPoolableMemoryPool<IMemoryPool, Explosion> { }
        private class SpawnVFXPool : MonoPoolableMemoryPool<IMemoryPool, SpawnVFX> { }
    }
}