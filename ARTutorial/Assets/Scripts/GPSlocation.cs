using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Android;
using System;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.XR.ARFoundation;


public class GPSlocation : MonoBehaviour
{
    [HideInInspector]
    public double latitude = 0;
    [HideInInspector]
    public double longitude = 0;

    [HideInInspector]
    public float delay;
    [HideInInspector]
    public float maxtime = 5.0f;

    [HideInInspector]
    public bool receiveGPS = false;

    double detailed_num = 1.0;

    private void Start()
    {
        StartCoroutine(Gps_man());
        Input.compass.enabled = true;
    }

    void Update()
    {

    }
    IEnumerator Gps_man()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) // 권한 요청하기  // GPS 요청 
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        // 권한이 거부되었을 때 처리
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Location permission denied by the user.");
            yield break; // 코루틴 종료
        }

        Input.location.Start(0.1f, 0.1f); ;
        while (Input.location.status == LocationServiceStatus.Initializing && delay < maxtime)
        {
            yield return new WaitForSeconds(1);
            delay++;
        }

        receiveGPS = true;

        while (receiveGPS)
        {
            latitude = (double)Input.location.lastData.latitude;
            longitude = (double)Input.location.lastData.longitude;

            Debug.Log($"Current Location: Latitude = {latitude}, Longitude = {longitude}");

            yield return new WaitForSeconds(0.2f);
        }
    }

    // GPS 중지 처리 (앱 비활성화나 종료 시)
    void OnDisable()
    {
        if (Input.location.isEnabledByUser)
        {
            Input.location.Stop(); // GPS 중지
            Debug.Log("GPS Service stopped.");
        }
    }
}
