using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Wheel[] wheels;
    public bool willDoSuspension;

    [Header("Car Specs")]
    public float wheelBase; //all in metres
    public float rearTrack;
    public float turnRadius;
    public float speedMult = 25000;

    [Header("Inputs")]
    public float steerInput;

    private float ackermannAngleLeft;
    private float ackermannAngleRight;
    void Update(){

        steerInput = Input.GetAxisRaw("Horizontal");

        if (steerInput > 0) // right
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
        }
        else if (steerInput < 0) // left
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
        }
        else //0
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        bool allWheelsAreOnGround = true;
        foreach (Wheel w in wheels)
        {

            if (!w.isOnGround)
            {
                allWheelsAreOnGround = false;
            }

            if (w.wheelFrontLeft)
            {
                w.steerAngle = ackermannAngleLeft;
            }
            if (w.wheelFrontRight)
            {
                w.steerAngle = ackermannAngleRight;
            }
        }
        if (allWheelsAreOnGround)
        {
            willDoSuspension = true;
        }
        else
        {
            willDoSuspension = false;
        }

    }
}
