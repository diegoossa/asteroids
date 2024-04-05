using Unity.Entities;
using Unity.Mathematics;

namespace DO.Asteroids
{
    /// <summary>
    /// Level bounds component.
    /// </summary>
    public struct LevelBounds : IComponentData
    {
        public float4 Bounds;
    }
}