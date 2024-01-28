using Unity.Entities;

namespace DO.Asteroids
{
    public struct PlayerInput : IComponentData
    {
        public float Rotation;
        public bool Thrust;
        public bool Shoot;
    }
}