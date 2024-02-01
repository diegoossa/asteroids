using Unity.Entities;
using UnityEngine;

namespace DO.Asteroids
{
    /// <summary>
    /// Authoring component for the weapon. 
    /// </summary>
    public class WeaponAuthoring : MonoBehaviour
    {
        public GameObject BulletPrefab;
        public float Cooldown = 0.2f;
        public float Spread = 0;

        public class WeaponBaker : Baker<WeaponAuthoring>
        {
            public override void Bake(WeaponAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Weapon
                {
                    BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                    Cooldown = authoring.Cooldown,
                    Spread = authoring.Spread
                });
            }
        }
        
    }
}