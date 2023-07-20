using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERotate { RotX, RotY, RotZ }
public class Rotate_Object : MonoBehaviour
{
    [Header("Rotate Data")]
    [Header("=======================================")]
    [SerializeField] float rotate_Speed;
    public ERotate rotate_Type;

    private Vector3 mousePos;
    private Vector3 offset;
    private Vector3 rotation;
    private bool isRotate;
    private bool isCurrect;

    [Header("Child Node")]
    [Header("=======================================")]
    public Node[] child_Nodes;

    [Header("ETC Data")]
    [Header("=======================================")]
    public Animator rotator_Anim;
    public bool isControl = true;
    public GameObject light_Object;

    //Sound
    private bool _isStop;

    void Awake()
    {
        child_Nodes = GetComponentsInChildren<Node>();
        TryGetComponent(out rotator_Anim);
    }

    void Update()
    {
        if (!isControl) return;

        //���콺�� ��� ���� ��
        if (isRotate)
        {
            _isStop = false;
            offset = (Input.mousePosition - mousePos);

            //�ڵ� ���� ����
            isCurrect = false;

            //Rotator Type�� ���� ȸ�� ���� ����
            switch (rotate_Type)
            {
                case ERotate.RotX:
                    rotation.x = -(offset.x + offset.y) * Time.deltaTime * rotate_Speed;
                    break;
                case ERotate.RotY:
                    rotation.y = -(offset.x + offset.y) * Time.deltaTime * rotate_Speed;
                    break;
                case ERotate.RotZ:
                    rotation.z = -(offset.x + offset.y) * Time.deltaTime * rotate_Speed;
                    break;
            }

            transform.Rotate(rotation);

            mousePos = Input.mousePosition;
        }

        //���콺�� ���� ���� ��
        else
        {
            //�ڵ� ���� �ѱ� (1ȸ ����)
            if (!isCurrect)
                Auto_Rotate_Euler();

            //Sound ȣ��
            if (!_isStop)
            {
                Sound_Manager.Instance.StopAll_SFX();
                int num = Random.Range(0, 4);
                switch (num)
                {
                    case 0:

                        Sound_Manager.Instance.PlaySE("Rotator_01");

                        break;
                    case 1:

                        Sound_Manager.Instance.PlaySE("Rotator_02");

                        break;
                    case 2:

                        Sound_Manager.Instance.PlaySE("Rotator_03");

                        break;
                    case 3:

                        Sound_Manager.Instance.PlaySE("Rotator_04");

                        break;
                }
                _isStop = true;
            }
        }
    }

    //���콺�� ���� �� 
    private void OnMouseDown()
    {
        isRotate = true;
        mousePos = Input.mousePosition;
    }

    //���콺 �� ��
    private void OnMouseUp()
    {
        isRotate = false;
    }

    //==================================================== Basic Method / CallBack Method =======================================================

    //Rotator ���� �ڵ� ����
    public void Auto_Rotate_Euler()
    {
        //Ÿ�Կ� ���� ���� ����
        float cur_Angle = Current_Rotate(rotate_Type);

        //��ǥ ���� ����
        float target_Angle;

        //���� ������ ��ǥ �������� ã��
        for (int i = 270; i >= 0; i -= 90)
        {
            if (cur_Angle > i)
            {
                if (cur_Angle > i + 45)
                {
                    target_Angle = i + 90;
                }
                else
                {
                    target_Angle = i;
                }
                Debug.Log(target_Angle);

                //���� �ڵ����� �޼���
                Auto_Euler(cur_Angle, target_Angle);
            }
        }
    }

    //Enum ���� ���� ������ �� ���� ����
    public float Current_Rotate(ERotate rotate)
    {
        Vector3 current_Euler = transform.eulerAngles;

        switch (rotate)
        {
            case ERotate.RotX:
                return current_Euler.x;
            case ERotate.RotY:
                return current_Euler.y;
            case ERotate.RotZ:
                return current_Euler.z;
        }
        return 0f;
    }

    void Auto_Euler(float start, float end)
    {
        // int������ ������ ���� ���� �� ��ǥ ������ ȸ�� (+ ����)
        if ((int)transform.localRotation.x != (int)end)
            transform.localEulerAngles = new Vector3(start + Time.deltaTime * rotate_Speed, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        // int������ ������ ���� ���� �� ��ǥ ������ ȸ�� (- ����)
        else
            transform.localEulerAngles = new Vector3(start - Time.deltaTime * rotate_Speed, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        // ��� ���� ������ ȸ�� �� �޼��� ����
        if (transform.localEulerAngles.x >= end - 0.3f && transform.localEulerAngles.x <= end + 0.3f) isCurrect = true;

    }
}