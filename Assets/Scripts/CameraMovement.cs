using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject vcam1;
    [SerializeField]
    private Transform startFollowPoint;
    [SerializeField]
    private BackgroundMovement bgMovement;

    private CinemachineVirtualCamera vcam1CVC;

    private void Start()
    {
        vcam1CVC = vcam1.GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        
        bgMovement.Follow();
        vcam1CVC.Follow = transform;
        
    }
}
