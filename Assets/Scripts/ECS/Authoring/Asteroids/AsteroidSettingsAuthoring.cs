using System;
using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    
    
    public class AsteroidSettingsAuthoring : MonoBehaviour
    {
        [Serializable]
        public struct StageSettings
        {
            public float Speed;
            public float Scale;
            public float RotationSpeed;
            public int Score;
        }
        
        public StageSettings[] Stages;

        public class AsteroidSettingsBaker : Baker<AsteroidSettingsAuthoring>
        {
            public override void Bake(AsteroidSettingsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                // Add Stage Settings
                var stageBuffer = AddBuffer<Stage>(entity);
                foreach (var stage in authoring.Stages)
                {
                    stageBuffer.Add(new Stage
                    {
                        Speed = stage.Speed,
                        Scale = stage.Scale,
                        RotationSpeed = stage.RotationSpeed,
                        Score = stage.Score
                    });
                }
            }
        }
    }
}