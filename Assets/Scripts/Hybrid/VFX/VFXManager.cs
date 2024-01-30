using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    public class VFXManager : IInitializable
    {
        private Explosion.Factory _explosionFactory;
        
        public VFXManager(Explosion.Factory explosionFactory)
        {
            _explosionFactory = explosionFactory;
        }

        public void Initialize()
        {
            HybridSignalBus.Instance.OnSpawnExplosion += SpawnExplosion;
        }

        private void SpawnExplosion(float2 position)
        {
            var explosion = _explosionFactory.Create();
            explosion.transform.position = new Vector3(position.x, position.y, 0);
        }
    }
}