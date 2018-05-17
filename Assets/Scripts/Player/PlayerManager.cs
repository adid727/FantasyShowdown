using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Added
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class PlayerManager : NetworkBehaviour
{
    [SerializeField]
    ToggleEvent m_toggleLocal;
    [SerializeField]
    ToggleEvent m_toggleShared;
    [SerializeField]
    ToggleEvent m_toggleRemote;
    [SerializeField]
    float m_respawnTime = 5.0f;
    [SerializeField]
    ThirdPersonController m_controller;

    //Camera
    [Header("Camera Settings")]
    [SerializeField]
    float m_cameraDistance = 5.0f;
    [SerializeField]
    float m_cameraHeight = 2.5f;
    //Private Camera Variables
    Transform m_mainCamera;
    Vector3 m_cameraOffset;

    void Start ()
    {
        //Don't use other player's camera
        if (!isLocalPlayer)
        {
            m_controller.enabled = false;
            return;
        }

        m_mainCamera = Camera.main.transform;
        m_cameraOffset = new Vector3(0.0f, m_cameraHeight, -m_cameraDistance);
        MoveCamera();
    }

    void FixedUpdate()
    {
        if (m_mainCamera != null)
        {
            //Update the camera
            MoveCamera();
        }
    }

    void MoveCamera()
    {
        m_mainCamera.position = transform.position;
        m_mainCamera.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        m_mainCamera.Translate(m_cameraOffset);
        m_mainCamera.LookAt(transform);
    }

    void EnablePlayer()
    {
        m_toggleShared.Invoke(true);

        if (isLocalPlayer)
        {
            m_toggleLocal.Invoke(true);
        }
        else
        {
            m_toggleRemote.Invoke(true);
        }
    }

    void DisablePlayer()
    {
        m_toggleShared.Invoke(false);

        if (isLocalPlayer)
        {
            m_toggleLocal.Invoke(false);
        }
        else
        {
            m_toggleRemote.Invoke(false);
        }
    }
}
