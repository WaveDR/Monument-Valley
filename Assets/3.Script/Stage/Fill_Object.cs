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


    //마우스 누를 때
    private void OnMouseDown()
    {
        clickPoint = Input.mousePosition;
    }

    //마우스 땔 때
    private void OnMouseUp()
    {
        _isStop = false;
    }

    //마우스 드래그로 오브젝트 위치 조정
    private void OnMouseDrag()
    {
        Vector3 diff = Input.mousePosition - clickPoint;
        Vector3 pos = transform.localPosition;

        pos.y += diff.y * Time.deltaTime * upDownSpeed;

        //목표 위치로 오브젝트 LocalPosition 조정
        transform.localPosition = new Vector3(transform.localPosition.x, Object_Cur_Pos(pos.y), transform.localPosition.z);

        //Sound Effect 실행
        if(transform.localPosition.y.Equals(Object_Cur_Pos(pos.y)) && !_isStop)
        {
            Play_SFX_Object(Object_Cur_Pos(pos.y));
        } 

        //ClickPoint 초기화
        clickPoint = Input.mousePosition;
    }

    //==================================================== Basic Method / CallBack Method =======================================================


    // 맞춰야할 0.0n 위치 값
    private float Object_Cur_Pos(float transformY)
    {
        //현재 LocalPostion.y의 값을 가져오기
        float object_PosY = transformY;

        //가져온 Y값을 0.01로 formet
        double round_Y = Math.Round(object_PosY,2); 

        //double 변수를 float로 형변환
        float round_To_Float = (float)round_Y;

        //형변환된 float 변수의 최소, 최소값 지정
        float clamp_Round = Mathf.Clamp(round_To_Float, min_PosY, max_PosY);
        return clamp_Round;
    }

    // 오브젝트의 위치값에 따른 Sound Effect 호출 
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
