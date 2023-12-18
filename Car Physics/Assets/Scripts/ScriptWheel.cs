using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScriptWheel : MonoBehaviour
{
    public bool wheelFrontLeft;
    public bool wheelFrontRight;
    public bool wheelRearLeft;
    public bool wheelRearRight;



    private Rigidbody rb;
    [Header("Suspension")]
    public float restlength;
    public float springTravel;
    public float springstiffness;
    public float damperstiffness;

    private float minlength;
    private float maxlength;
    private float lastlength;
    private float springlength;
    private float springVelocity;
    private float springForce;
    private float damperForce;

    

    [Header("Wheel")]
    public float steerAngle;
    public float steerTime;

    private Vector3 suspensionForce;
    private float wheelAngle;
    private Vector3 wheelVelocityLS;

    private float Fx;
    private float Fy;

    [Header("Wheel")]
    public float wheelRadius;
    
    
    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
        minlength = restlength - springTravel;
        maxlength = restlength + springTravel;
    }


    void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation= Quaternion.Euler(Vector3.up*wheelAngle);
        Debug.DrawRay(transform.position, -transform.up * (springlength + wheelRadius), Color.green);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxlength + wheelRadius))
        {
            lastlength = springlength;
            springlength = hit.distance - wheelRadius;
            springlength = Mathf.Clamp(springlength, minlength, maxlength);
            springVelocity = (lastlength - springlength) / Time.fixedDeltaTime;
            springForce = springstiffness * (restlength - springlength);
            damperForce = damperstiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;

            wheelVelocityLS = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            Fx = Input.GetAxis("Vertical")*springForce;
            Fy = wheelVelocityLS.x * springForce;

            rb.AddForceAtPosition(suspensionForce+(Fx*transform.forward)+(Fy*-transform.right), hit.point);
        }
    }
}