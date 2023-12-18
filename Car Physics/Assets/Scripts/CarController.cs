using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public ScriptWheel[] wheels;

    [Header("Car Specs")]
    public float wheelBase;
    public float rearTrack;
    public float turnRadius;

    [Header("Steer Input")]
    public float steerInput;


    public float ackermannAngleRight;
    public float ackermannAngleLeft;

   
    void Update()
    {

        steerInput = Input.GetAxis("Horizontal");
     if(steerInput>0)  //Turn right
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
        }
     else if(steerInput < 0)  //Turn left
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
        }
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

     foreach(ScriptWheel w in wheels)
        {
            if(w.wheelFrontLeft)
                w.steerAngle=ackermannAngleLeft;
            if (w.wheelFrontRight)
                w.steerAngle = ackermannAngleRight;
        }
    }
}
