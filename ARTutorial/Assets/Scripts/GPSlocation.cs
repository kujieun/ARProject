using System.Collections;
using UnityEngine;
using UnityEngine.Android;


public class GPSlocation : MonoBehaviour
{
    public double latitude = 0; // ���� ����
    public double longitude = 0; // ���� �浵

    private float delay; // �ʱ�ȭ ��� �ð�
    private float maxtime = 5.0f; // �ִ� ��� �ð�

    private bool receiveGPS = false; // GPS ���� ����

    private void Start()
    {
        StartCoroutine(Gps_man()); // GPS ����
        Input.compass.enabled = true; // ��ħ�� Ȱ��ȭ
    }

    IEnumerator Gps_man()
    {
        // GPS ���� ��û
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        // ������ �źεǾ��� ��
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Location permission denied by the user.");
            yield break; // �ڷ�ƾ ����
        }

        // ��ġ ���� ����
        Input.location.Start(0.1f, 0.1f); ; // GPS�� 0.1�ʸ��� ��ġ���� ��������, �ּ� 0.1m�� ��Ȯ���� ����
        while (Input.location.status == LocationServiceStatus.Initializing && delay < maxtime)
        {
            yield return new WaitForSeconds(1); // �ʱ�ȭ ���
            delay++; // ��� �ð� ����
        }

        receiveGPS = true;

        while (receiveGPS)
        {
            // ���� ��ġ�� ���� �� �浵 ������Ʈ
            latitude = (double)Input.location.lastData.latitude;
            longitude = (double)Input.location.lastData.longitude;

            Debug.Log($"Current Location: Latitude = {latitude}, Longitude = {longitude}");

            yield return new WaitForSeconds(0.2f);
        }
    }

    // �� ��Ȱ��ȭ, ����� GPS ����
    void OnDisable()
    {
        if (Input.location.isEnabledByUser)
        {
            Input.location.Stop();
            Debug.Log("GPS Service stopped.");
        }
    }
}
