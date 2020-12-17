using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour
{
    [SerializeField] float thrustForce = 2.0f;
    [SerializeField] float rotationStrength = 30f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip finishClip;
    [SerializeField] AudioClip deathClip;

    [SerializeField] ParticleSystem thrust;
    [SerializeField] ParticleSystem finish;
    [SerializeField] ParticleSystem death;
    // Start is called before the first frame update

    Rigidbody rigidBody;
    AudioSource audioSource;
    bool collisionsDisabled = false;

    enum State { Alive, Dying, Trancending }
    State state = State.Alive;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
        
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            LoadNextScene();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled) { return; }
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                // Go to next level
                FinishSequence();
                break;
            default:
                // restart this level.
                DeathSequence();
                break;
        }
    }

    private void FinishSequence()
    {
        state = State.Trancending;
        audioSource.Stop();
        audioSource.PlayOneShot(finishClip);
        finish.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void DeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathClip);
        death.Play();
        Invoke("ReloadCurrentScene", levelLoadDelay);
    }

    private void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1)% SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Rotate(Vector3.back * Input.GetAxis("Horizontal") * Time.deltaTime * rotationStrength);
        }
        rigidBody.freezeRotation = false;
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            if(!thrust.isPlaying)
                thrust.Play();
        } else
        {
            audioSource.Stop();
            thrust.Stop();
        }

        //if(Input.GetKeyUp(KeyCode.Space))

    }
}
