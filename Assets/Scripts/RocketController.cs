using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour
{
    [SerializeField] float thrustForce = 2.0f;
    [SerializeField] float rotationStrength = 30f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip finishClip;
    [SerializeField] AudioClip deathClip;

    [SerializeField] ParticleSystem thrust;
    [SerializeField] ParticleSystem finish;
    [SerializeField] ParticleSystem death;
    // Start is called before the first frame update

    Rigidbody rigidBody;
    AudioSource audioSource;

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
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }
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
        Invoke("LoadNextScene", 1f);
    }

    private void DeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathClip);
        death.Play();
        Invoke("LoadPrevScene", 1f);
    }

    private void LoadPrevScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
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
            rigidBody.AddRelativeForce(Vector3.up * thrustForce);
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
