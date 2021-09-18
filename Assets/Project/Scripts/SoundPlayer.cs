using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts
{
    public class SoundPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private float RandomizePitch()
        {
            return Random.Range(0.9f, 1.1f);
        }

        public void Play(AudioClip clip)
        {
            _audioSource.pitch = RandomizePitch();
            _audioSource.PlayOneShot(clip);
        }
    }
}
