using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class GameManagerAuthoring : MonoBehaviour
    {
        [Header("Ship Spawn Settings")]
        public GameObject ShipPrefab;
        public float TimeToSpawn;
        public bool ShouldSpawn;
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
                    ShouldSpawn = authoring.ShouldSpawn,
                });
                AddComponent(entity, new Lives
                    {InitialLives = authoring.InitialLives, CurrentLives = authoring.InitialLives});
                AddComponent(entity, new Score());
            }
        }
    }
} 