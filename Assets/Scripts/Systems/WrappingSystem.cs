using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    public partial struct WrappingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<LevelBounds>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var levelBounds = SystemAPI.GetSingleton<LevelBounds>();
            Debug.Log("Level bounds: " + levelBounds.Bounds);
        }
    }
}