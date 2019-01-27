using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene2Trigger : MonoBehaviour
{
    public GameObject activateOnTrigger;
    //public CinemachineVirtualCamera mainVCam;
    //public CinemachineVirtualCamera cutsceneVCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController.Instance.isInputAllowed = false;
        //mainVCam.enabled = false;
        //cutsceneVCam.enabled = true;
        activateOnTrigger.SetActive(true);
    }


}
