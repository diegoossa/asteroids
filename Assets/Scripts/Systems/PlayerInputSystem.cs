using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class PlayerInputSystem : SystemBase
    {
        private InputActions _inputActions;
        
        protected override void OnCreate()
        {
            _inputActions = new InputActions();
        }

        protected override void OnStartRunning()
        {
            _inputActions.Enable();
        }

        protected override void OnStopRunning()
        {
            _inputActions.Disable();
        }

        protected override void OnUpdate()
        {
            var shoot = _inputActions.Game.Shoot.IsPressed();
            var thrust = _inputActions.Game.Thrust.IsPressed();
            var rotation = _inputActions.Game.Rotate.ReadValue<float>();
            
            Debug.Log($"Shoot > {shoot} ::: Thrust > {thrust} ::: Rotation > {rotation}");
        }
    }

    public struct PlayerInput : IComponentData
    {
        public float Rotation;
        public bool Thrust;
        public bool Shoot;
    }
}