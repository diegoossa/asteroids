using System;
using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class AsteroidsSpawnerAuthoring : MonoBehaviour
    {
        public GameObject asteroidPrefab;
        public int numAsteroids = 8;

        public class AsteroidsSpawnerBaker : Baker<AsteroidsSpawnerAuthoring>
        {
            public override void Bake(AsteroidsSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new AsteroidsSpawner
                    {
                        AsteroidPrefab = GetEntity(authoring.asteroidPrefab, TransformUsageFlags.Dynamic),
                        NumAsteroids = authoring.numAsteroids
                    });
            }
        }
    }
}