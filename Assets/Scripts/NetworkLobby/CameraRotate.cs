using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {

    [Header("Scene Camera Properties")]
    [SerializeField]
    Transform sceneCamera;
    [SerializeField]
    float cameraRotationRadius = 24.0f;
    [SerializeField]
    float cameraRotationSpeed = 3.0f;
    [SerializeField]
    bool canRotate = true;

    float rotation;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!canRotate)
        {
            return;
        }

        rotation += cameraRotationSpeed * Time.deltaTime;
        if(rotation >= 360.0f)
        {
            rotation -= 360.0f;
        }

        sceneCamera.position = new Vector3(0,0,0);
        sceneCamera.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        sceneCamera.Translate(0.0f, cameraRotationRadius, -cameraRotationRadius);
        sceneCamera.LookAt(Vector3.zero);
	}
}
