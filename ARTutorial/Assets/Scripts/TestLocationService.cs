using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class TestLocationService : MonoBehaviour
{
    public RawImage mapImage; // 구글 맵 이미지를 표시할 RawImage
    private const string mapUrl = "https://maps.googleapis.com/maps/api/staticmap?";
    private const string apiKey = "AIzaSyCfIvnP-Av9cnfftvbnmWje6sP2loNyz_Q";

    IEnumerator Start()
    {
        // 하드코딩된 좌표 설정 (예: 서울 시청)
        float latitude = 37.5665f; // 위도
        float longitude = 126.978f; // 경도

        // 구글 맵 URL 생성
        string url = $"{mapUrl}center={latitude},{longitude}&zoom=15&size=640x640&maptype=roadmap&markers=color:red%7Clabel:C%7C{latitude},{longitude}&key={apiKey}";

        // 구글 맵 이미지를 다운로드하고 RawImage에 적용
        yield return StartCoroutine(DownloadMapImage(url));
    }

    private IEnumerator DownloadMapImage(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            // 요청 보내기
            yield return webRequest.SendWebRequest();

            // 오류 처리
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error downloading map image: {webRequest.error}");
            }
            else
            {
                // 다운로드한 이미지를 RawImage에 적용
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                mapImage.texture = texture;
            }
        }
    }

    private void OnDisable()
    {
        // 위치 서비스 중지
        Input.location.Stop();
    }
}
