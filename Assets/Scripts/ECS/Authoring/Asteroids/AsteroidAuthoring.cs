using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public class AsteroidAuthoring : MonoBehaviour
    {
        public float Radius = 0.5f;
        
        [Header("Stage")]
        public int Stage;
        public int MaxStage = 2;
        public float StageSpeedMultiplier = 2f;
        public float StageScaleMultiplier = 0.5f;
        
        public class AsteroidBaker : Baker<AsteroidAuthoring>
        {
            public override void Bake(AsteroidAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Asteroid());
                AddComponent(entity, new Speed());
                AddComponent(entity, new Direction());
                AddComponent(entity, new RotationSpeed());
                AddComponent(entity, new PhysicsRadius {Radius = authoring.Radius});
                AddComponent(entity, new Enemy());
                AddComponent(entity, new Wrap());
                AddComponent(entity, new Stage
                {
                    Value = authoring.Stage,
                    MaxStage = authoring.MaxStage,
                    SpeedMultiplier = authoring.StageSpeedMultiplier,
                    ScaleMultiplier = authoring.StageScaleMultiplier,
                });
            }
        }
    }
}