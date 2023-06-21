using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private float speed;

    // 카메라의 처리는 LateUpdate에서 처리하도록 한다.
    private void LateUpdate()
    {
        // Lerp를 사용해서 카메라서 끊김없이 서서히 따라가도록 만들어준다.
        gameObject.transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * speed);
        // 카메라가 바라볼 대상을 정해준다.
        gameObject.transform.LookAt(playerHead.transform);
    }
}
