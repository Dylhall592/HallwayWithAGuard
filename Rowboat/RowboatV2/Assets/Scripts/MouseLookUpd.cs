using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookUpd : MonoBehaviour
{
    //look sensitivity
    public float speedV = 2.0f;
    public float speedH = 2.0f;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    //limits of the camera
    private float minValue = -90f;
    private float maxValue = 90f;
    private bool hide;
    private bool row;

    void Start()
    {
      Cursor.lockState = CursorLockMode.Locked;
      hide = false;
      row = false;
    }

    void Update()
    {
        if (hide == false & row == false)
        {
            //rotationY affects limited cam movement on X axis
            rotationY += speedV * Input.GetAxis("Mouse X");
            rotationX -= speedH * Input.GetAxis("Mouse Y");
            rotationX = Mathf.Clamp(rotationX, minValue, maxValue);
            transform.eulerAngles = new Vector3(rotationX, rotationY, 0);
        }
        else
        {

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Hiding~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        if (Input.GetKey(KeyCode.H))
        {
            hide = true;
            //makes camera look down
            Quaternion target = Quaternion.Euler(45, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, 10 * Time.deltaTime);
            rotationY = transform.rotation.y;
            rotationX = transform.rotation.x;
            Debug.Log("hiding");
        }
        else
        {
            hide = false;
            Debug.Log("not hiding");
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Rowing~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        if (Input.GetKey(KeyCode.W))
        {
            row = true;
        }
        else
        {
            row = false;
        }
    }
}
