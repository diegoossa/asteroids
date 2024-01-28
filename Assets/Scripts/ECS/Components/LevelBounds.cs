using Unity.Entities;
using Unity.Mathematics;

namespace DO.Asteroids
{
    public struct LevelBounds : IComponentData
    {
        public float4 Bounds;
    }
}