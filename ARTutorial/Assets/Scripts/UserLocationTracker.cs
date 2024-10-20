using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class UserLocationTracker : MonoBehaviour
{
    public RawImage mapRawImage; // 지도가 표시될 이미지
    public Image userLocationMarker; // 사용자의 위치를 표시할 이미지

    [Header("Map SET")]

    private string strBaseURL = "https://maps.googleapis.com/maps/api/staticmap?";

    public int zoom = 16;
    public int mapWidth = 600;
    public int mapHeight = 600;
    private string strAPIKey = "AIzaSyCfIvnP-Av9cnfftvbnmWje6sP2loNyz_Q";

    public GPSlocation GPSlocation; // GPS 가져올 다른 클래스
    private double latitude = 0;    // 현재 위도
    private double longitude = 0;   // 현재 경도

    // 위치가 변경되었는지 감지하기 위해 이전값을 저장
    private double save_latitude = 0;  // 이전 위도 저장
    private double save_longitude = 0; // 이전 경도 저장

    void Start()
    {
        mapRawImage = GetComponent<RawImage>();
        StartCoroutine(WaitForSecond()); // 1초마다 위치 업데이트 확인

    }

    private void Update()
    {
        latitude = GPSlocation.latitude;   // GPS 클래스에서 가져온 현재 위도
        longitude = GPSlocation.longitude; // GPS 클래스에서 가져온 현재 경도

        UpdateUserLocationMarker(); // 위치 마커 업데이트

        print("location" + latitude + " " + longitude);
    }

    private void UpdateUserLocationMarker()
    {
        // 사용자 방향에 따라 마커 회전
        float userRotation = Input.compass.trueHeading; // 나침반에서 사용자 방향 가져오기
        userLocationMarker.rectTransform.rotation = Quaternion.Euler(0, 0, -userRotation); // 회전 설정
    }


    IEnumerator WaitForSecond()
    {
        while (true)
        {
            // 현재 위도 및 경도가 이전 값과 다르면 지도 업데이트
            if (save_latitude != latitude || save_longitude != longitude)
            {
                save_latitude = latitude; // 현재 위도 저장
                save_longitude = longitude; // 현재 경도 저장
                StartCoroutine(LoadMap()); // 지도 로드
            }
            print("3초");
            yield return new WaitForSeconds(3f);
        }
    }

    // 구글맵 이미지 로드
    IEnumerator LoadMap()
    {
        // 구글맵 API 호출을 위한 URL
        string url = strBaseURL + "center=" + latitude + "," + longitude +
            "&zoom=" + zoom.ToString() +
            "&size=" + mapWidth.ToString() + "x" + mapHeight.ToString()
            + "&key=" + strAPIKey;

        url = UnityWebRequest.UnEscapeURL(url);
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url); // 텍스처 요청 생성

        yield return req.SendWebRequest(); // 요청 전송 및 응답 대기

        // Error handling 추가
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            mapRawImage.texture = DownloadHandlerTexture.GetContent(req); // 다운로드 한 지도 이미지를 RawImage에 적용
        }
    }
}
