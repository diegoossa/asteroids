using System;
using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class AsteroidsSpawnerAuthoring : MonoBehaviour
    {
        public GameObject AsteroidPrefab;
        public int NumAsteroids = 8;
        public float Radius = 0.5f;
        [Range(1, 1000)]
        public uint RandomSeed = 42;                 // Expose RandomSeed for testing purposes

        public class AsteroidsSpawnerBaker : Baker<AsteroidsSpawnerAuthoring>
        {
            public override void Bake(AsteroidsSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new AsteroidSpawner
                    {
                        AsteroidPrefab = GetEntity(authoring.AsteroidPrefab, TransformUsageFlags.Dynamic),
                        NumAsteroids = authoring.NumAsteroids,
                        Radius = authoring.Radius,
                        RandomSeed = authoring.RandomSeed
                    });
            }
        }
    }
}