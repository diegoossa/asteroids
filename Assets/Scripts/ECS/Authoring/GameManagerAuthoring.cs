using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class GameManagerAuthoring : MonoBehaviour
    {
        [Header("Ship Spawn Settings")]
        public GameObject ShipPrefab;
        public float TimeToSpawn;
        [Header("Lives")]
        public int InitialLives;

        public class GameManagerBaker : Baker<GameManagerAuthoring>
        {
            public override void Bake(GameManagerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new GameManager
                {
                    ShipPrefab = GetEntity(authoring.ShipPrefab, TransformUsageFlags.Dynamic),
                    TimeToSpawn = authoring.TimeToSpawn,
                    ShouldSpawn = true
                });
                AddComponent(entity, new Lives
                    {InitialLives = authoring.InitialLives, CurrentLives = authoring.InitialLives});
                AddComponent(entity, new Score {Value = 0});
            }
        }
    }
} 