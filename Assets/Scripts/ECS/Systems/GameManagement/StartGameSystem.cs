using DO.Asteroids.Hybrid;
using Unity.Entities;

namespace DO.Asteroids
{
    public partial struct StartGameSystem : ISystem, ISystemStartStop
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameManager>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            if(HybridMessageBus.Instance != null)
                HybridMessageBus.Instance.OnGameStateChange += OnStartGame;
        }

        private void OnStartGame(GameState gameState)
        {
            if (gameState == GameState.Play)
            {
                var gameManager = SystemAPI.GetSingletonRW<GameManager>();
                gameManager.ValueRW.ShouldSpawn = true;
                gameManager.ValueRW.GameState = GameState.Play;
            }
        }

        public void OnStopRunning(ref SystemState state) { }
    }
}