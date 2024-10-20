using System.Collections;
using UnityEngine;
using UnityEngine.Android;


public class GPSlocation : MonoBehaviour
{
    public double latitude = 0; // 현재 위도
    public double longitude = 0; // 현재 경도

    private float delay; // 초기화 대기 시간
    private float maxtime = 5.0f; // 최대 대기 시간

    private bool receiveGPS = false; // GPS 수신 여부

    private void Start()
    {
        StartCoroutine(Gps_man()); // GPS 시작
        Input.compass.enabled = true; // 나침반 활성화
    }

    IEnumerator Gps_man()
    {
        // GPS 권한 요청
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        // 권한이 거부되었을 때
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Location permission denied by the user.");
            yield break; // 코루틴 종료
        }

        // 위치 서비스 시작
        Input.location.Start(0.1f, 0.1f); ; // GPS가 0.1초마다 위치정보 업데이터, 최소 0.1m의 정확도로 수신
        while (Input.location.status == LocationServiceStatus.Initializing && delay < maxtime)
        {
            yield return new WaitForSeconds(1); // 초기화 대기
            delay++; // 대기 시간 증가
        }

        receiveGPS = true;

        while (receiveGPS)
        {
            // 현재 위치의 위도 및 경도 업데이트
            latitude = (double)Input.location.lastData.latitude;
            longitude = (double)Input.location.lastData.longitude;

            Debug.Log($"Current Location: Latitude = {latitude}, Longitude = {longitude}");

            yield return new WaitForSeconds(0.2f);
        }
    }

    // 앱 비활성화, 종료시 GPS 중지
    void OnDisable()
    {
        if (Input.location.isEnabledByUser)
        {
            Input.location.Stop();
            Debug.Log("GPS Service stopped.");
        }
    }
}
