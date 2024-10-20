using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class UserLocationTracker : MonoBehaviour
{
    public RawImage mapRawImage; // ������ ǥ�õ� �̹���
    public Image userLocationMarker; // ������� ��ġ�� ǥ���� �̹���

    [Header("Map SET")]

    private string strBaseURL = "https://maps.googleapis.com/maps/api/staticmap?";

    public int zoom = 16;
    public int mapWidth = 600;
    public int mapHeight = 600;
    private string strAPIKey = "AIzaSyCfIvnP-Av9cnfftvbnmWje6sP2loNyz_Q";

    public GPSlocation GPSlocation; // GPS ������ �ٸ� Ŭ����
    private double latitude = 0;    // ���� ����
    private double longitude = 0;   // ���� �浵

    // ��ġ�� ����Ǿ����� �����ϱ� ���� �������� ����
    private double save_latitude = 0;  // ���� ���� ����
    private double save_longitude = 0; // ���� �浵 ����

    void Start()
    {
        mapRawImage = GetComponent<RawImage>();
        StartCoroutine(WaitForSecond()); // 1�ʸ��� ��ġ ������Ʈ Ȯ��

    }

    private void Update()
    {
        latitude = GPSlocation.latitude;   // GPS Ŭ�������� ������ ���� ����
        longitude = GPSlocation.longitude; // GPS Ŭ�������� ������ ���� �浵

        UpdateUserLocationMarker(); // ��ġ ��Ŀ ������Ʈ

        print("location" + latitude + " " + longitude);
    }

    private void UpdateUserLocationMarker()
    {
        // ����� ���⿡ ���� ��Ŀ ȸ��
        float userRotation = Input.compass.trueHeading; // ��ħ�ݿ��� ����� ���� ��������
        userLocationMarker.rectTransform.rotation = Quaternion.Euler(0, 0, -userRotation); // ȸ�� ����
    }


    IEnumerator WaitForSecond()
    {
        while (true)
        {
            // ���� ���� �� �浵�� ���� ���� �ٸ��� ���� ������Ʈ
            if (save_latitude != latitude || save_longitude != longitude)
            {
                save_latitude = latitude; // ���� ���� ����
                save_longitude = longitude; // ���� �浵 ����
                StartCoroutine(LoadMap()); // ���� �ε�
            }
            print("3��");
            yield return new WaitForSeconds(3f);
        }
    }

    // ���۸� �̹��� �ε�
    IEnumerator LoadMap()
    {
        // ���۸� API ȣ���� ���� URL
        string url = strBaseURL + "center=" + latitude + "," + longitude +
            "&zoom=" + zoom.ToString() +
            "&size=" + mapWidth.ToString() + "x" + mapHeight.ToString()
            + "&key=" + strAPIKey;

        url = UnityWebRequest.UnEscapeURL(url);
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url); // �ؽ�ó ��û ����

        yield return req.SendWebRequest(); // ��û ���� �� ���� ���

        // Error handling �߰�
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            mapRawImage.texture = DownloadHandlerTexture.GetContent(req); // �ٿ�ε� �� ���� �̹����� RawImage�� ����
        }
    }
}
