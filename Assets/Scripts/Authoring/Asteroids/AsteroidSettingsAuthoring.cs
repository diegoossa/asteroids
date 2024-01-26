using Unity.Entities;
using Unity.Entities.UI;
using Unity.Mathematics;
using UnityEngine;

namespace DO.Asteroids
{
    public class AsteroidSettingsAuthoring : MonoBehaviour
    {
        [MinMax(0, 100)]
        public Vector2 MinMaxSpeed;
        public float2 MinMaxRotationSpeed;

        public class AsteroidSettingsBaker : Baker<AsteroidSettingsAuthoring>
        {
            public override void Bake(AsteroidSettingsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity,
                    new AsteroidSettings
                    {
                        MinMaxSpeed = authoring.MinMaxSpeed,
                        MinMaxRotationSpeed = authoring.MinMaxRotationSpeed
                    });
            }
        }
    }
}