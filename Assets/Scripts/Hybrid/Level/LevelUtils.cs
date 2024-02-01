using Unity.Mathematics;
using UnityEngine;

namespace DO.Asteroids.Utils
{
    /// <summary>
    /// Set of utilities for working with the level.
    /// </summary>
    public static class LevelUtils
    {
        public static float4 CalculateBounds(Camera camera)
        {
            var verticalExtent = camera.orthographicSize;
            var horizontalExtent = verticalExtent * camera.aspect;
            return new float4(-horizontalExtent, horizontalExtent, -verticalExtent, verticalExtent);
        }
    }
}