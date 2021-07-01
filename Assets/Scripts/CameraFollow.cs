using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public CinemachineFreeLook cam;
    public float rotateXSpeed = 3f;
    float mouseX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        mouseX = Input.GetAxis("Mouse X");
        cam.m_XAxis.m_InputAxisValue = mouseX * rotateXSpeed;
    }
}
