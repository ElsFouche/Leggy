using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    //Testing to Direct Particles towards Leggy

    //Assign Legg's transform in Inspector

    public Transform target;

    private ParticleSystem pSystem;

    private ParticleSystem.Particle[] particles;

    float speed;

    void Start()
    {
        pSystem = GetComponent<ParticleSystem>();

        speed = 5;
    }

    void Update()
    {
        if (target == null) return;

        int maxParticles = pSystem.main.maxParticles;

        if (particles == null || particles.Length < maxParticles)

            particles = new ParticleSystem.Particle[maxParticles];

        int particleCount = pSystem.GetParticles(particles);

        for (int i = 0; i < particleCount; i++)
        {
            Vector3 direction = (target.position - particles[i].position).normalized;

            //Adjust speed

            Quaternion targetRotation = Quaternion.LookRotation(target.position - particles[i].position);
            pSystem.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

            particles[i].velocity = direction * 3.0f;
        }

        pSystem.SetParticles(particles, particleCount);
    }

    public void particleStream()
    {
        pSystem.Emit(30);
    }
}