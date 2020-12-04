using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] float thrustForce = 2.0f;
    [SerializeField] float rotationStrength = 30f;
    // Start is called before the first frame update

    Boolean dead = false;
    Rigidbody rigidBody;
    AudioSource audioSource;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
            Thrusting();
            Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            default:
                dead = true;
                break;
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetAxis("Horizontal") != 0 && !dead)
        {
            transform.Rotate(Vector3.back * Input.GetAxis("Horizontal") * Time.deltaTime * rotationStrength);
        }
        rigidBody.freezeRotation = false;
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space) && !dead)
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustForce);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}
