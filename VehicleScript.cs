using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour
{
    public List<Axle> axles;
    public float maxMotorTorque;
    public float maxSteerAngle;

    //temp Movement values
    public float steering;
    public float motor;
    public float brake;

    //GameObjects
    public GameObject lifter;
    public GameObject holder;
    public GameObject forks;

    //Bounds for lift
    public float minHeight;
    public float maxHeight;
    public float minRotation;
    public float maxRotation;
    public float maxOffset;

    //speed Values for lift
    public float liftSpeed; 
    public float rotateSpeed;
    public float offsetSpeed;

    //Temp values for lift
    float lift; 
    float rot;
    float off;

    public float newRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motor = 0;
        brake = 0;

        lift = 0;
        rot = 0;
        off = 0;
        
        GetInputs();
        
        foreach (Axle axle in axles) //Apply vehicle forces
        {
            if (axle != null)
            {
                if (axle.motor)
                {
                    axle.rightWheel.motorTorque = motor;
                    axle.leftWheel.motorTorque = motor;
                }
                if (axle.steering)
                {
                    axle.rightWheel.steerAngle = steering;
                    axle.leftWheel.steerAngle = steering;
                }
                if (axle.brake)
                {
                    axle.rightWheel.brakeTorque = brake;
                    axle.leftWheel.brakeTorque = brake;
                }
            }
        }

        //Apply arm rotation
        newRotation = lifter.transform.localRotation.eulerAngles.x;
        newRotation += rot;

        if ((newRotation > maxRotation) && (newRotation < 180))
        {
            newRotation = maxRotation;
        } else if ((newRotation < minRotation) && (newRotation > 180))
        {
            newRotation = minRotation;
        }
        lifter.transform.localRotation = Quaternion.Euler(newRotation, 0.0f, 0.0f);

        //Apply forks height
        float newHeight = holder.transform.localPosition.y;
        newHeight += lift;

        if (newHeight > maxHeight) 
        { 
            newHeight = maxHeight; 
        } else if (newHeight < minHeight) 
        { 
            newHeight = minHeight; 
        }
        holder.transform.localPosition = new Vector3(0.0f, newHeight, 0.588f);

        //Apply forks side offset
        float newOffset = forks.transform.localPosition.x;
        newOffset += off;

        if (newOffset > maxOffset)
        {
            newOffset = maxOffset;
        } else if (newOffset < -maxOffset)
        {
            newOffset = -maxOffset;
        }
        forks.transform.localPosition = new Vector3(newOffset, 0.0f, 2.041f);
    }

    void GetInputs() //Get keys and set temp values
    {
        // chassis Movement
        if (Input.GetKey(KeyCode.UpArrow))
        {
            motor = maxMotorTorque;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            brake = maxMotorTorque * 2;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            steering--;

            if (steering < -maxSteerAngle)
            {
                steering = -maxSteerAngle;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            steering++;

            if (steering > maxSteerAngle)
            {
                steering = maxSteerAngle;
            }
        }

        //arm movement
        if (Input.GetKey(KeyCode.A))
        {
            lift = liftSpeed;
        } else if (Input.GetKey(KeyCode.Z))
        {
            lift = -liftSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            rot = rotateSpeed;
        } else if (Input.GetKey(KeyCode.X))
        {
            rot = -rotateSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            off = -offsetSpeed;
        } else if (Input.GetKey(KeyCode.C))
        {
            off = offsetSpeed;
        }
    }
}

[System.Serializable]
public class Axle //Axle info
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public bool brake;
}