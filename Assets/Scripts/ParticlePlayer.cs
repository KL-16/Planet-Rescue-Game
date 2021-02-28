using UnityEngine;
using System.Collections;

public class ParticlePlayer : MonoBehaviour 
{

	public ParticleSystem[] allParticles;
	public float lifetime = 1f;
    public bool destroyImmediately = true;

	void Start () 
	{
		allParticles = GetComponentsInChildren<ParticleSystem>();

        if (destroyImmediately)
        {
            Destroy(gameObject, lifetime);
        }
	}
	
	public void Play()
	{
		foreach (ParticleSystem ps in allParticles)
		{
			ps.Stop();
			
			ps.Play();
		}

        Destroy(gameObject, lifetime);
	}

	public void Stop()
	{
		foreach (ParticleSystem ps in allParticles)
		{
			ps.Stop();
		}

		Destroy(gameObject, lifetime);
	}

	public void PlayPoints(MatchValue value)
	{
		foreach (ParticleSystem ps in allParticles)
		{
			ParticleSystem.MainModule ma = ps.main;

			if(value == MatchValue.Green)
			{
				ma.startColor = Color.green;
			}

			else if (value == MatchValue.Blue)
			{
				ma.startColor = Color.blue;
			}
			else if (value == MatchValue.Yellow)
			{
				ma.startColor = Color.yellow;
			}
			else if (value == MatchValue.Red)
			{
				ma.startColor = Color.red;
			}
			else if (value == MatchValue.Purple)
			{
				ma.startColor = Color.magenta;
			}
			else if (value == MatchValue.White)
			{
				ma.startColor = Color.white;
			}
			else if (value == MatchValue.Barrel || value == MatchValue.BrokenBarrel || value == MatchValue.Litter)
			{
				ma.startColor = Color.grey;
			}

			else
			{
				ma.startColor = Color.cyan;
			}


			ps.Stop();

			ps.Play();
		}

		Destroy(gameObject, lifetime);
	}

}
