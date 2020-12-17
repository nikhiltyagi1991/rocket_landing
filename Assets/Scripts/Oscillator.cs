using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [Range(1, 10)] [SerializeField] float period = 2f;
    [Range(0,1)] [SerializeField] float movementFactor;

    Vector3 startingPos;

    bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (collided) return;

        // Set movement factor
        float cycles = 0;
        if(period != 0)
        {
            cycles = Time.time / period; // grows from 0
        }
        
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f; // shift from -1 - 1 to -0.5 - 0.5 and then add 0.5 to have 0-1 range

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Player":
                collided = true;
                break;
        }
    }
}
