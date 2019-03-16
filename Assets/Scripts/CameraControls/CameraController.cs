using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private int scrollSpeed =3 ;
    private int travel;
    [SerializeField] private int minCameraSize;
    [SerializeField] private int maxCameraSize;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");
        float rotationValue = Input.GetAxis("Rotation");
        float scrollWheelValue = Input.GetAxis("Mouse ScrollWheel");

        float cameraSize = Camera.main.orthographicSize;
        
        if (scrollWheelValue > 0f && cameraSize > minCameraSize)
        {
            Camera.main.orthographicSize -= scrollSpeed;
        }
        else if (scrollWheelValue < 0f && cameraSize < maxCameraSize)
        {
            travel = travel + scrollSpeed;
            Camera.main.orthographicSize += scrollSpeed;
        }
        
        if(Camera.current != null)
        {
            transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue), Space.Self);
            transform.Rotate(0,rotationValue, 0, Space.World);
        }
        
        
        
    }
}
