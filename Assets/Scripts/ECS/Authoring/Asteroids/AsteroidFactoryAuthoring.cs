using System;
using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class AsteroidFactoryAuthoring : MonoBehaviour
    {
        public GameObject AsteroidPrefab;
        public int NumAsteroids = 8;
        [Range(1, 1000)]
        public uint RandomSeed = 42;                 // Expose RandomSeed for testing purposes

        public class AsteroidFactoryBaker : Baker<AsteroidFactoryAuthoring>
        {
            public override void Bake(AsteroidFactoryAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new AsteroidSpawner
                    {
                        AsteroidPrefab = GetEntity(authoring.AsteroidPrefab, TransformUsageFlags.Dynamic),
                        NumAsteroids = authoring.NumAsteroids,
                        RandomSeed = authoring.RandomSeed
                    });
            }
        }
    }
}