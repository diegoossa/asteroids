using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DO.Asteroids
{
    public class AsteroidSettingsAuthoring : MonoBehaviour
    {
        public float2 MinMaxSpeed;
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