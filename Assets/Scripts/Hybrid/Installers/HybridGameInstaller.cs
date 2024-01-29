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
            InstallSignals();
        }

        private void InstallFactories()
        {
            Container.BindFactory<Explosion, Explosion.Factory>()
                .FromPoolableMemoryPool<Explosion, ExplosionPool>(poolBinder => poolBinder
                    .WithInitialSize(8)
                    .FromComponentInNewPrefab(_settings.ExplosionPrefab)
                    .UnderTransformGroup("VFX"));
        }
        
        private void InstallSignals()
        {
            
        }
        
        [Serializable]
        public class Settings
        {
            public GameObject ExplosionPrefab;
        }

        private class ExplosionPool : MonoPoolableMemoryPool<IMemoryPool, Explosion> { }
    }
}