using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class TestLocationService : MonoBehaviour
{
    public RawImage mapImage; // ���� �� �̹����� ǥ���� RawImage
    private const string mapUrl = "https://maps.googleapis.com/maps/api/staticmap?";
    private const string apiKey = "AIzaSyCfIvnP-Av9cnfftvbnmWje6sP2loNyz_Q";

    IEnumerator Start()
    {
        // �ϵ��ڵ��� ��ǥ ���� (��: ���� ��û)
        float latitude = 37.5665f; // ����
        float longitude = 126.978f; // �浵

        // ���� �� URL ����
        string url = $"{mapUrl}center={latitude},{longitude}&zoom=15&size=640x640&maptype=roadmap&markers=color:red%7Clabel:C%7C{latitude},{longitude}&key={apiKey}";

        // ���� �� �̹����� �ٿ�ε��ϰ� RawImage�� ����
        yield return StartCoroutine(DownloadMapImage(url));
    }

    private IEnumerator DownloadMapImage(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            // ��û ������
            yield return webRequest.SendWebRequest();

            // ���� ó��
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error downloading map image: {webRequest.error}");
            }
            else
            {
                // �ٿ�ε��� �̹����� RawImage�� ����
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                mapImage.texture = texture;
            }
        }
    }

    private void OnDisable()
    {
        // ��ġ ���� ����
        Input.location.Stop();
    }
}
