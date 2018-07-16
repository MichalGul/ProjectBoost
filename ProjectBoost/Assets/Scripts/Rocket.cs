﻿using UnityEngine;
using UnityEngine.SceneManagement;



public class Rocket : MonoBehaviour {
    //todo fix lightningh bug
    Rigidbody rigidbodyComponent;
    AudioSource audiosourceComponent;


    [SerializeField] //Leave field to the inspector
   public float rcsThrust = 1000f;

    [SerializeField] //Leave field to the inspector
    public float mainThrust = 100f;

    [SerializeField]
    public AudioClip mainEngine; 
    public AudioClip deathSound;
    public AudioClip loadLevel;

    [SerializeField]
    public ParticleSystem mainEngineParticles;
    public ParticleSystem deathSoundParticles;
    public ParticleSystem loadLevelParticles;

    [SerializeField] public float levelLoadDelay = 2f;

    enum State
    {
        Alive,
        Dying,
        Transcending
    }
    //Current state of the player rocket
    State state = State.Alive;

	// Use this for initialization
	void Start ()
    {
        //referencja do komponentu rigidbody przypietego to tego game objectu do ktorego jest przyczepiony skrypt
        rigidbodyComponent = GetComponent<Rigidbody>();
        audiosourceComponent = GetComponent<AudioSource>();
        //audiosourceComponent.PlayOneShot(loadLevel);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }
    }

    private void RespondToRotateInput()
    {

        rigidbodyComponent.freezeRotation = true; // take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
            
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidbodyComponent.freezeRotation = false; // resume physics



    }
    private void RespondToThrustInput()
    {
        if(state != State.Alive) { return; }

        if (Input.GetKey(KeyCode.Space) )
        {
            ApplyThrust();
        }
        else
        {
            audiosourceComponent.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = Time.deltaTime * mainThrust;

        rigidbodyComponent.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audiosourceComponent.isPlaying) // zeby sie nie nakładały dzwieki z kolejnych ramek
        {
            audiosourceComponent.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    void OnCollisionEnter(Collision other)
    {
        if (state != State.Alive) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                print("Friendly Collision");
                break;
            case "Finish":
                StartSuccesSequence();
                break;
            default:
                StardDeathSequence();
                break;
        }

    }

    private void StardDeathSequence()
    {
        state = State.Dying;
        audiosourceComponent.Stop();
        audiosourceComponent.PlayOneShot(deathSound);
        deathSoundParticles.Play();
        print("Your ass is dead!");
        Invoke("RestartGame", levelLoadDelay);
    }

    private void StartSuccesSequence()
    {
        state = State.Transcending;
        audiosourceComponent.Stop();
        audiosourceComponent.PlayOneShot(loadLevel);
        loadLevelParticles.Play();
        Invoke("LoadnextScene", levelLoadDelay);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadnextScene()
    {
        SceneManager.LoadScene(1); // Allow for more than nex levels
    }
}
