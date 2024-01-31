using UnityEngine;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    public class VFXManager : IInitializable
    {
        private Explosion.Factory _explosionFactory;
        private SpawnVFX.Factory _spawnVFXFactory;
        
        public VFXManager(Explosion.Factory explosionFactory, SpawnVFX.Factory spawnVFXFactory)
        {
            _explosionFactory = explosionFactory;
            _spawnVFXFactory = spawnVFXFactory;
        }

        public void Initialize()
        {
            HybridSignalBus.Instance.OnSpawnExplosion += SpawnExplosion;
            HybridSignalBus.Instance.OnSpawnShip += SpawnInstantiateVFX;
        }

        private void SpawnExplosion(Vector2 position)
        {
            var explosion = _explosionFactory.Create();
            explosion.transform.position = new Vector3(position.x, position.y, -1f);
        }
        
        private void SpawnInstantiateVFX()
        {
            var spawnVFX = _spawnVFXFactory.Create();
        }
    }
}