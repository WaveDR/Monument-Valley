using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fill_Object : MonoBehaviour
{
    //Sound
    private bool _isStop;

    [Header("Y Position Value")]
    [Header("=======================================")]
    private Vector3 clickPoint;
    private float upDownSpeed = 0.2f;
    public float min_PosY;
    public float max_PosY;


    //���콺 ���� ��
    private void OnMouseDown()
    {
        clickPoint = Input.mousePosition;
    }

    //���콺 �� ��
    private void OnMouseUp()
    {
        _isStop = false;
    }

    //���콺 �巡�׷� ������Ʈ ��ġ ����
    private void OnMouseDrag()
    {
        Vector3 diff = Input.mousePosition - clickPoint;
        Vector3 pos = transform.localPosition;

        pos.y += diff.y * Time.deltaTime * upDownSpeed;

        //��ǥ ��ġ�� ������Ʈ LocalPosition ����
        transform.localPosition = new Vector3(transform.localPosition.x, Object_Cur_Pos(pos.y), transform.localPosition.z);

        //Sound Effect ����
        if(transform.localPosition.y.Equals(Object_Cur_Pos(pos.y)) && !_isStop)
        {
            Play_SFX_Object(Object_Cur_Pos(pos.y));
        } 

        //ClickPoint �ʱ�ȭ
        clickPoint = Input.mousePosition;
    }

    //==================================================== Basic Method / CallBack Method =======================================================


    // ������� 0.0n ��ġ ��
    private float Object_Cur_Pos(float transformY)
    {
        //���� LocalPostion.y�� ���� ��������
        float object_PosY = transformY;

        //������ Y���� 0.01�� formet
        double round_Y = Math.Round(object_PosY,2); 

        //double ������ float�� ����ȯ
        float round_To_Float = (float)round_Y;

        //����ȯ�� float ������ �ּ�, �ּҰ� ����
        float clamp_Round = Mathf.Clamp(round_To_Float, min_PosY, max_PosY);
        return clamp_Round;
    }

    // ������Ʈ�� ��ġ���� ���� Sound Effect ȣ�� 
    private void Play_SFX_Object(float num)
    {
        Sound_Manager.Instance.StopAll_SFX();
        switch (num)
         {
             case -0.02f:
                 Sound_Manager.Instance.PlaySE("Fill_01");
                 break;
             case 0.01f:
                 Sound_Manager.Instance.PlaySE("Fill_02");
                 break;
            case 0.03f:
                Sound_Manager.Instance.PlaySE("Fill_03");
                break;
            case 0.06f:
                Sound_Manager.Instance.PlaySE("Fill_04");
                break;
            case 0.07f:
                Sound_Manager.Instance.PlaySE("Fill_05");
                break;
            default:
                Sound_Manager.Instance.PlaySE("Fill_06");
                break;
        }
        _isStop = true;
    }
}
