using DO.Asteroids.Utils;
using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    /// <summary>
    /// This is a MonoBehaviour that will be used to create an entity with a LevelBounds component.
    /// </summary>
    public class LevelBoundsAuthoring : MonoBehaviour
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
            
            // Destroy this MonoBehaviour, since it's no longer needed.
            Destroy(gameObject);
        }
    }
}