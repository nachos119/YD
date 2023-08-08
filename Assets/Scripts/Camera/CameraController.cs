using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController
{
    private const float HALF = 0.5f;
    private const float correctionMap = 0.5f;

    private readonly Vector3 correctionCameraPosition = new Vector3(0f, 0f, -10f);

    private Camera mainCamera = null;

    // ī�޶� ���� ���� �÷��̾� ���ǵ�� ���� �ӵ��� �ִ°� ���ƺ���
    private float cameraMoveSpeed = 10f;

    private float cameraWidth = 0f;
    private float cameraHeight = 0f;

    private float mapHalfSizeWidth = 0f;
    private float mapHalfSizeHeight = 0f;

    private bool isLoopCamera = false;

    public CameraController(Camera _camera)
    {
        mainCamera = _camera;

        // TODO:: camera projection �����
        // ī�޶� ������
        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = cameraHeight * Screen.width / Screen.height;
    }

    /// <summary>
    /// Camera Fix Check
    /// </summary>
    public bool SetFixCamera { set { isLoopCamera = value; } }

    /// <summary>
    /// Camera Show Update
    /// </summary>
    /// <param name="_transform"></param>   Player Position
    public void UpdateCamera(Transform _transform)
    {
        // �ɼ� delegate ��� �ʿ�
        if (isLoopCamera == true)
        {
            mainCamera.transform.position = _transform.position + correctionCameraPosition;
        }
        else
        {
            // �⺻
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, _transform.position + correctionCameraPosition,
                                      Time.deltaTime * cameraMoveSpeed);
        }

        var mainCameraPos = mainCamera.transform.position;
        // ī�޶� �ִ� ���α���
        float limitCameraWidth = mapHalfSizeWidth - cameraWidth;
        float clampWidth = Mathf.Clamp(mainCameraPos.x, -limitCameraWidth, limitCameraWidth);

        // ī�޶� �ִ� ����
        float limitCameraHeight = mapHalfSizeHeight - cameraHeight;
        float clampHeight = Mathf.Clamp(mainCameraPos.y, -limitCameraHeight, limitCameraHeight);

        mainCamera.transform.position = new Vector3(clampWidth, clampHeight, mainCameraPos.z);
    }

    public void SetMapSize(float _width, float _height)
    {
        mapHalfSizeWidth = _width * HALF + correctionMap;
        mapHalfSizeHeight = _height * HALF + correctionMap;
    }

    /// <summary>
    /// Camera Shake
    /// </summary>
    /// <param name="_time"></param>    Shake Time
    /// <param name="_range"></param>   Shake Range
    /// <param name="_speed"></param>   Shake Speed
    public async void CameraShake(float _time, float _range, float _speed = 0.5f)
    {
        var cameraPos = mainCamera.transform.localPosition;
        float timer = 0f;

        while (timer <= _time)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, Random.insideUnitSphere * _range + cameraPos, _speed);

            timer += Time.deltaTime * _speed;
            await UniTask.WaitForEndOfFrame();
        }

        mainCamera.transform.localPosition = cameraPos;
    }
}