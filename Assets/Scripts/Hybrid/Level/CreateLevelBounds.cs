using DO.Asteroids.Utils;
using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    /// <summary>
    /// This is a MonoBehaviour that will be used to create an entity with a LevelBounds component.
    /// </summary>
    public class CreateLevelBounds : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        private void Start()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, new LevelBounds
            {
                Bounds = LevelUtils.CalculateBounds(mainCamera)
            });
            
            // TODO: Change LevelBounds when the Game Window is resized.
        }
    }
}