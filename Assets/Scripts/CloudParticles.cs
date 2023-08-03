using UnityEngine;

public class CloudParticles : MonoBehaviour
{
	public float Margin = 4f;

	private ParticleSystem ParticleSystem;

	private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

	private void Start()
	{
		ParticleSystem = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		int num = ParticleSystem.GetParticles(particles);
		Camera camera = SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera;
		Vector3 vector = camera.ViewportToWorldPoint(new Vector3(1f, 1f, -1f));
		Vector3 vector2 = camera.ViewportToWorldPoint(new Vector3(1f, 0f, -1f));
		for (int i = 0; i < num; i++)
		{
			Vector3 position = particles[i].position;
			if (position.x > vector.x + Margin)
			{
				particles[i].remainingLifetime = 0f;
				continue;
			}
			Vector3 position2 = particles[i].position;
			if (position2.y > vector.y + Margin)
			{
				Vector3 position3 = particles[i].position;
				position3.y = vector2.y - (position3.y - vector.y);
				particles[i].position = position3;
			}
		}
		ParticleSystem.SetParticles(particles, num);
	}
}
