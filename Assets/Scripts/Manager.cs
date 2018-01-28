using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour 
{
	public AudioSource efxSource;    
	public AudioSource barkSource;					//Drag a reference to the audio source which will play the sound effects.
	public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
	public static Manager instance = null;     //Allows other scripts to call functions from SoundManager.             
	public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

	public static float timeLeft = 60;

	public EnemySpawner spawner1;
	public EnemySpawner spawner2;

	public static bool lost = false;
	public static bool won = false;

	HomeBase homebase;

	int lastint = 10;


	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
	}


	//Used to play single sound clips.
	public void PlaySingle(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		efxSource.clip = clip;

		//Play the clip.
		efxSource.Play ();
	}

	void Update()
	{
		while(!won && !lost)
		{
			timeLeft -= Time.deltaTime;
			if(timeLeft <= 0 && won != true)
			{
				Debug.Log ("YAY");
				won = true;
				spawner1.shouldSpawn = false;
				spawner2.shouldSpawn = false;
				SceneManager.LoadScene (2);
			}

			if (homebase == null)
			{
				homebase = GameObject.FindGameObjectWithTag ("Home Base").GetComponent<HomeBase> ();				
			}

			if(homebase.health <= 0 && lost != true)
			{
				Debug.Log ("BOO");
				lost = true;
				spawner1.shouldSpawn = false;
				spawner2.shouldSpawn = false;
				SceneManager.LoadScene (2);
			}

			if((int)Mathf.Floor (Manager.GetTimeLeft() / 60) + 1 < lastint)
			{
				lastint = (int)Mathf.Floor (Manager.GetTimeLeft () / 60);
				spawner1.spawnDelay--;
				spawner2.spawnDelay--;
			}
			
		}
	}


	//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
	public void RandomizeSfx (params AudioClip[] clips)
	{
		if (efxSource.isPlaying) {
			return;
		}
		//Generate a random number between 0 and the length of our array of clips passed in.
		int randomIndex = Random.Range(0, clips.Length);

		//Choose a random pitch to play back our clip at between our high and low pitch ranges.
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		//Set the pitch of the audio source to the randomly chosen pitch.
		efxSource.pitch = randomPitch;

		//Set the clip to the clip at our randomly chosen index.
		efxSource.clip = clips[randomIndex];

		//Play the clip.
		efxSource.Play();
	}

	//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
	public void RandomizeBarks (params AudioClip[] clips)
	{

		if (barkSource.isPlaying) {
			return;
		}
		int rng = Random.Range (0, 75);
		if (rng != 1) {
			return;
		}
		//Generate a random number between 0 and the length of our array of clips passed in.
		int randomIndex = Random.Range(0, clips.Length);

		//Choose a random pitch to play back our clip at between our high and low pitch ranges.
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		//Set the pitch of the audio source to the randomly chosen pitch.
		barkSource.pitch = randomPitch;

		//Set the clip to the clip at our randomly chosen index.
		barkSource.clip = clips[randomIndex];

		//Play the clip.
		barkSource.Play();
	}

	public static float GetTimeLeft()
	{
		return timeLeft;
	}
}