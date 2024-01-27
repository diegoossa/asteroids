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
            RequireForUpdate<PlayerInput>();
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
            var playerInput = SystemAPI.GetSingletonRW<PlayerInput>();
            
            var shoot = _inputActions.Game.Shoot.IsPressed();
            var thrust = _inputActions.Game.Thrust.IsPressed();
            var rotation = _inputActions.Game.Rotate.ReadValue<float>();
            
            playerInput.ValueRW.Rotation = rotation;
            playerInput.ValueRW.Thrust = thrust;
            playerInput.ValueRW.Shoot = shoot;

            //Debug.Log($"Shoot > {shoot} ::: Thrust > {thrust} ::: Rotation > {rotation}");
        }
    }
}