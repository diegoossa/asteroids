using Unity.Entities;

namespace DO.Asteroids
{
    /// <summary>
    /// Player input component for the ship.
    /// </summary>
    public struct PlayerInput : IComponentData
    {
        public float Rotation;
        public bool Thrust;
        public bool Shoot;
    }
}