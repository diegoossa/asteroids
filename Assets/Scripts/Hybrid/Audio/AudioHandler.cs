using System;
using UnityEngine;
using Zenject;

namespace DO.Asteroids.Hybrid
{
    public class AudioHandler : IInitializable, IDisposable
    {
        private readonly Settings _settings;
        private readonly AudioSource _audioSource;
        
        public AudioHandler(AudioSource audioSource, Settings settings)
        {
            _settings = settings;
            _audioSource = audioSource;
        }
        
        public void Dispose()
        {
            HybridSignalBus.OnSpawnExplosion -= OnSpawnExplosion;
        }
        
        public void Initialize()
        {
            HybridSignalBus.OnSpawnExplosion += OnSpawnExplosion;
        }
        
        private void OnSpawnExplosion(Vector2 position)
        {
            _audioSource.PlayOneShot(_settings.ExplosionSound);
        }
        
        [Serializable]
        public class Settings
        {
            public AudioClip ExplosionSound;
            public AudioClip SpawnSound;
        }
    }
}