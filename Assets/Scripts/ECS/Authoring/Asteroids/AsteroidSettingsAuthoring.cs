using Unity.Entities;
using Unity.Entities.UI;
using Unity.Mathematics;
using UnityEngine;

namespace DO.Asteroids
{
    public class AsteroidSettingsAuthoring : MonoBehaviour
    {
        [MinMax(0, 100)]
        public float Speed;
        public float RotationSpeed;
        public float Radius = 0.5f;

        public class AsteroidSettingsBaker : Baker<AsteroidSettingsAuthoring>
        {
            public override void Bake(AsteroidSettingsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity,
                    new AsteroidSettings
                    {
                        Speed = authoring.Speed,
                        RotationSpeed = authoring.RotationSpeed,
                        Radius = authoring.Radius,
                    });
            }
        }
    }
}