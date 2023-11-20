using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public CarController car;
    public bool isOnGround;

    private Rigidbody rb;

    public bool wheelFrontLeft;
    public bool wheelFrontRight;
    public bool wheelRearLeft;
    public bool wheelRearRight;

    [Header("Suspension")]
    public float restLength;
    public float maxSpringTravel;
    public float springStiffness;

    public float damperStiffness;

    private float maxLength;
    private float minLength;
    private float springLength;
    private float springForce;
    private float lastLength;
    private float springVelocity;
    private float damperForce;


    private Vector3 suspensionForce;

    [Header("Wheel")]
    public float steerAngle;
    public float steerTime;
    private float steerAngleLerp;
    public float radius;


    public Vector3 wheelVelocityLS; //Local-Space
    private float Fx;
    private float Fy;

    [Header("Visual")]
    private Vector3 standardWheelPos;
    public Transform visualWheel;
    public float wheelSpeed;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        minLength = restLength - maxSpringTravel;
        //maxLength = restLength + maxSpringTravel;
        maxLength = restLength + 0.1f;
        standardWheelPos = visualWheel.localPosition;
    }

    private void Update()
    {
        steerAngleLerp = Mathf.Lerp(steerAngleLerp, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * steerAngleLerp);
        //visualWheel.localEulerAngles = new(visualWheel.localEulerAngles.x + wheelVelocityLS.z, steerAngleLerp, 0);
        visualWheel.localEulerAngles = new(0, steerAngleLerp + 90, visualWheel.localEulerAngles.z + wheelVelocityLS.z*0.2f);
    }
    void FixedUpdate()
    {

        if (Physics.Raycast(transform.position, -car.transform.up, out RaycastHit hit, maxLength + radius))
        {
            isOnGround = true;
            wheelVelocityLS = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point)); //Velocity of hit point
            Fx = Input.GetAxis("Vertical") * car.speedMult;
            //(Input.GetAxisRaw("Accel") - Input.GetAxisRaw("Decel"))
            Fy = wheelVelocityLS.x * springForce;

            //visualWheel.position = new Vector3(hit.point.x, hit.point.y + radius * transform.up.y, hit.point.z);
            visualWheel.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) -(springLength*car.transform.up);

            if (car.willDoSuspension)
            {
                lastLength = springLength; //Store last calculation's length before updating
                springLength = hit.distance - radius; //Dist from origin to wheel center
                springLength = Mathf.Clamp(springLength, minLength, maxLength);

                springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
                damperForce = damperStiffness * springVelocity; //Damper calculation

                springForce = springStiffness * (restLength - springLength);
                suspensionForce = (springForce + damperForce) * car.transform.up;
                rb.AddForceAtPosition(suspensionForce + (Fx * transform.forward) + (Fy * -transform.right), hit.point); //Calculate and apply final force
            }


        }
        else
        {
            isOnGround = false;
            visualWheel.localPosition = standardWheelPos;
        }
    }
    private void OnDrawGizmosSelected()
    {
        #region Suspension Gizmos

        Gizmos.color = Color.yellow;

        Vector3 rest = transform.position;
        rest.y = +restLength;
        Gizmos.DrawWireSphere(rest, 0.01f);

        Gizmos.color = Color.red;

        Vector3 up = new(rest.x, rest.y + maxSpringTravel, rest.z);
        Vector3 down = new(rest.x, rest.y - maxSpringTravel, rest.z);
        Gizmos.DrawLine(up, down);

        Gizmos.color = Color.green;

        Vector3 length = new(transform.position.x, transform.position.y - springLength, transform.position.z);
        Gizmos.DrawLine(transform.position, length);
        #endregion
    }

}