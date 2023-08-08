using HSMLibrary.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasePlayerController
{
    private const float HALF = 0.5f;

    private float speed = 10f;

    private float mapSizeWidth = 0;
    private float mapSizeHeight = 0;
    //private Vector3 mapSize = Vector3.zero;

    public PlayerController(Transform _transform) : base(_transform)
    {
    }

    public Vector3 SetMapSize
    {
        set
        { 
            //mapSize = value * 0.5f;
            // TODO:: �ӽ÷� ĳ���� ũ��� scale�� ���� �����ʿ�
            mapSizeWidth = value.x * HALF - (transform.localScale.x + HALF); // * -> + �� ����..�� ���� �÷��̾� �ڱ� �ڽ��� ũ�� ������ŭ �� ��������...
            mapSizeHeight = value.y * HALF - (transform.localScale.y + HALF); // * -> + �� ����..�� ���� �÷��̾� �ڱ� �ڽ��� ũ�� ������ŭ �� ��������...
        }
    }

    public override void OnMove(float _rot, float _speed)
    {
        // TODO:: map check������ base ������� ����
        float speed = _speed * Time.deltaTime;

        Vector3 curPos = transform.position;
        curPos.x = Mathf.Clamp(curPos.x + (Mathf.Cos(_rot) * speed), -mapSizeWidth, mapSizeWidth);
        curPos.y = Mathf.Clamp(curPos.y + (Mathf.Sin(_rot) * speed), -mapSizeHeight, mapSizeHeight);

        transform.position = curPos;
    }

    public override void OnMove(Vector3 _dir, float _speed)
    {
        _speed *= speed;

        base.OnMove(_dir, _speed);
    }

    public Transform GetPlayerTransform { get { return transform; } }
}
