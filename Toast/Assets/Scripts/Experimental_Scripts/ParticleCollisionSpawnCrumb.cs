using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionSpawnCrumb : MonoBehaviour
{
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            MeshParticleSystem.instance.CreateParticle(collisionEvents[i].intersection);
        }
    }
}
