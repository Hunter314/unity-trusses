using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float Speed;
    public float startZoom;
    public Vector3 rotationPoint;
    private float currentZoom;
    // Start is called before the first frame update
    void Start()
    {
        Transform thisTransform = GetComponent<Transform>();
        currentZoom = startZoom;
        thisTransform.position = rotationPoint + new Vector3(0,0, -startZoom);
        thisTransform.rotation.SetLookRotation(toOrigin());

    }

    // Update is called once per frame
    Vector3 toOrigin()
    {
        Transform thisTransform = GetComponent<Transform>();

        return rotationPoint - thisTransform.position;
    }
    void Update()
    {

        Transform thisTransform = GetComponent<Transform>();
        //A is rotate around origin left
        //D is rotate around origin right
        //E is rotate around origin up
        //C is rotate around origin down
        //W is move away from origin
        //S is move towards origin
        thisTransform.RotateAround(rotationPoint, Vector3.up, -Input.GetAxis("Horizontal") * Speed * Time.deltaTime);
        thisTransform.RotateAround(rotationPoint, new Vector3(-toOrigin().z, 0, toOrigin().x), -Input.GetAxis("Vertical") * Speed * Time.deltaTime);
        currentZoom = currentZoom + Input.GetAxis("Zoom") * Speed * Time.deltaTime;
        thisTransform.position = rotationPoint - (toOrigin().normalized * currentZoom);
        //Just to be safe.
        thisTransform.rotation.SetLookRotation(toOrigin());

    }
}
