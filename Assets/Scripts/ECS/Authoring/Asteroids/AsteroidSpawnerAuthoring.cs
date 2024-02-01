using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    /// <summary>
    /// Asteroid Spawner Authoring
    /// </summary>
    public class AsteroidSpawnerAuthoring : MonoBehaviour
    {
        public GameObject AsteroidPrefab;
        public int NumAsteroids = 4;
        [Range(1, 1000)] public uint RandomSeed = 42; // Expose RandomSeed for testing purposes
        public bool ShouldSpawn = true;

        public class AsteroidFactoryBaker : Baker<AsteroidSpawnerAuthoring>
        {
            public override void Bake(AsteroidSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AsteroidSpawner
                {
                    AsteroidPrefab = GetEntity(authoring.AsteroidPrefab, TransformUsageFlags.Dynamic),
                    NumAsteroids = authoring.NumAsteroids,
                    RandomSeed = authoring.RandomSeed,
                    ShouldSpawn = authoring.ShouldSpawn
                });
            }
        }
    }
}