﻿using UnityEngine;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    /// <summary>
    /// Explosion VFX.
    /// </summary>
    public class Explosion : MonoBehaviour, IPoolable<IMemoryPool>
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private ParticleSystem vfx;
        private float _startTime;
        private IMemoryPool _pool;
        
        public void Update()
        {
            if (Time.realtimeSinceStartup - _startTime > lifeTime)
            {
                _pool.Despawn(this);
            }
        }
        
        public void OnDespawned() { }
        
        public void OnSpawned(IMemoryPool pool)
        {
            vfx.Clear();
            vfx.Play();
        
            _startTime = Time.realtimeSinceStartup;
            _pool = pool;
        }

        public class Factory : PlaceholderFactory<Explosion> { }
    }
}