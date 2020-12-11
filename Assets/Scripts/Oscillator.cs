using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period;
    [Range(0,1)] [SerializeField] float movementFactor;

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Set movement factor
        float cycles = Time.time / period; // grows from 0
        const float tau = Mathf.PI * 2;


        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
