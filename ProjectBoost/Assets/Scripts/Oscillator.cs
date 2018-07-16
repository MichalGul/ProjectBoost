using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField]
    public Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField]
    public float period = 2f;
    //TODO remove from inspector later
    [Range(0,1)]
    [SerializeField]
    float movementFactor;

    private Vector3 startingPos; //store for absolute movement

	// Use this for initialization
	void Start ()
    {

        startingPos = transform.position;

	}
	
	// Update is called once per frame
	void Update ()
    {
        try
        {
            //ilosc cykli ochronic przed period = 0
            float cycles = Time.time / period; //grows from 0

           if (period <= Mathf.Epsilon) { throw new DivideByZeroException(); }

            const float tau = Mathf.PI * 2f;// 2*PI 6.28
            float rawSinWave = Mathf.Sin(tau * cycles); //2Pi*f  = 2Pi/T  *time  -1 do 1

            movementFactor = (rawSinWave / 2f) + 0.5f;

            Vector3 offset = movementVector * movementFactor;
            transform.position = startingPos + offset;
        }
        catch(DivideByZeroException)
        {
            //print("Period set to 0, obstacle not moving");
            startingPos = transform.position;
        }
       
	}
}
