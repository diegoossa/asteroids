using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class RandomAsteroidSpawnerAuthoring : MonoBehaviour
    {
        public GameObject AsteroidPrefab;
        public int NumAsteroids = 8;
        [Range(1, 1000)]
        public uint RandomSeed = 42;                // Expose RandomSeed for testing purposes
        public bool ShouldSpawn = true;
            
        public class AsteroidFactoryBaker : Baker<RandomAsteroidSpawnerAuthoring>
        {
            public override void Bake(RandomAsteroidSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new AsteroidRandomSpawner
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