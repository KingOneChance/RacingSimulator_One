using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private float speed;

    // ī�޶��� ó���� LateUpdate���� ó���ϵ��� �Ѵ�.
    private void LateUpdate()
    {
        // Lerp�� ����ؼ� ī�޶� ������� ������ ���󰡵��� ������ش�.
        gameObject.transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * speed);
        // ī�޶� �ٶ� ����� �����ش�.
        gameObject.transform.LookAt(playerHead.transform);
    }
}
