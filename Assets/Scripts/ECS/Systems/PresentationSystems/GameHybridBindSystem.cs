// using DO.Asteroids.Hybrid;
// using Unity.Entities;
// using UnityEngine;
//
// namespace DO.Asteroids
// {
//     public partial class GameHybridBindSystem : SystemBase
//     { 
//         protected override void OnUpdate()
//         {
//             if(HybridMessageBus.Instance == null)
//             {
//                 Debug.Log("NO HYBRID MESSAGE BUS");
//                 return;
//             }
//             
//             Debug.Log("UPDATE FROM SYSTEM");
//             
//             HybridMessageBus.Instance.OnGameStateChange += OnGameStateChange;
//         }
//
//         private void OnGameStateChange(GameStateEnum state, int score)
//         {
//             Debug.Log("GAME STATE CHANGE FROM SYSTEM");
//         }
//     }
// }