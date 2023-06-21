using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum MoveState
{
    Stop,
    Go,
    Back,
}
public enum HandleState
{
    Front,
    Right,
    Left,
}
public class CarController : MonoBehaviour
{
    // ���� ���� ���� �κ� 4��
    [SerializeField] private Transform[] wheelPos = new Transform[4];
    [SerializeField] private WheelCollider[] wheels = new WheelCollider[4];
    [SerializeField] private float power = 100f; // ������ ȸ����ų ��
    [SerializeField] private float rot = 30f; // ������ ȸ�� ����
    [SerializeField] private float deAcc = 0.1f; // ������ �극��ũ �ӷ�
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MoveState state = MoveState.Stop;
    [SerializeField] private HandleState handleState = HandleState.Front;
    [SerializeField] private RawImage breakPaddle = null;
    [SerializeField] private RawImage gasPaddle = null;
    [SerializeField] private RectTransform handle = null;
    private void Start()
    {
        for (int i = 0; i < wheelPos.Length; i++)
        {
            wheels[i].transform.position = wheelPos[i].transform.position;
        }

        rb = GetComponent<Rigidbody>();
        // ���� �߽��� y�� �Ʒ��������� �����.
        rb.centerOfMass = new Vector3(0, -1, 0);
    }
    private void FixedUpdate()
    {
        WheelPosAndAni();
        if (Input.GetKey(KeyCode.Space) && state == MoveState.Go)
        {
            breakPaddle.color = new Color(255, 0, 0, 255);
            for (int k = 0; k < wheels.Length; k++)
            {
                // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
                if (wheels[k].motorTorque > 0)
                {
                    wheels[k].motorTorque -= power * deAcc;
                    rb.velocity -= rb.transform.forward * deAcc;
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    wheels[k].motorTorque = 0;
                    state = MoveState.Stop;
                }
            }
        }
        else if (Input.GetKey(KeyCode.Space) && state == MoveState.Back)
        {
            breakPaddle.color = new Color(255, 0, 0, 255);
            for (int k = 0; k < wheels.Length; k++)
            {
                // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
                if (wheels[k].motorTorque < 0)
                {
                    wheels[k].motorTorque += power * deAcc;
                    rb.velocity += rb.transform.forward * deAcc;
                }
                else
                {
                    wheels[k].motorTorque = 0;
                    rb.velocity = Vector3.zero;
                    state = MoveState.Stop;
                }
            }
        }
        if (state == MoveState.Stop)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                breakPaddle.color = new Color(255, 0, 0, 255);
            }
            else
            {
                breakPaddle.color = new Color(255, 255, 255, 255);
            }

            rb.velocity = Vector3.zero;
            for (int k = 0; k < wheels.Length; k++)
            {
                wheels[k].motorTorque = 0;
                rb.velocity = Vector3.zero;
                state = MoveState.Stop;
            }
        }

    }
    void WheelPosAndAni()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < wheels.Length; i++)
        {
            // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
            wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
            if (wheels[i].motorTorque > 0) state = MoveState.Go;
            else if (wheels[i].motorTorque < 0) state = MoveState.Back;

            if (Input.GetAxis("Vertical") != 0)
            {
                gasPaddle.color = new Color(255, 0, 0, 255);
            }
            else
            {
                gasPaddle.color = new Color(255, 255, 255, 255);
            }
        }
        for (int i = 0; i < 2; i++)
        {
            // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
            float horiz = Input.GetAxis("Horizontal");
            wheels[i].steerAngle = horiz * rot;
            if (horiz > 0)
            {
                if (handleState != HandleState.Right)
                {
                    handleState = HandleState.Right;
                    handle.Rotate(-Vector3.forward * 30);
                }
            }
            else if (horiz < 0)
            {
                if (handleState != HandleState.Left)
                {
                    handleState = HandleState.Left;
                    handle.Rotate(Vector3.forward * 30);
                }
            }
            else if (horiz == 0)
            {
                if (handleState != HandleState.Front)
                {
                    handleState = HandleState.Front;
                    handle.rotation = Quaternion.identity;
                }
            }
            else
                Debug.LogError("SomeThing Wrong . Question To Wonchan");
        }
        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelPos[i].transform.position = wheelPosition;
            wheelPos[i].transform.rotation = wheelRotation;
        }
    }
}
