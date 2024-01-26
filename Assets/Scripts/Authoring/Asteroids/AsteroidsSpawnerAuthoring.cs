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

        public class AsteroidsSpawnerBaker : Baker<AsteroidsSpawnerAuthoring>
        {
            public override void Bake(AsteroidsSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new AsteroidsSpawner
                    {
                        AsteroidPrefab = GetEntity(authoring.AsteroidPrefab, TransformUsageFlags.Dynamic),
                        NumAsteroids = authoring.NumAsteroids,
                        Radius = authoring.Radius
                    });
            }
        }
    }
}