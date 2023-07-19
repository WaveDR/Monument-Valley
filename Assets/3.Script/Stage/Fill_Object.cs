using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fill_Object : MonoBehaviour
{
    Vector3 clickPoint;
    float upDownSpeed = 0.2f;

    public float min_PosY;
    public float max_PosY;

    private bool _isStop;
    float Object_Cur_Pos(float transformY)
    {
        float object_PosY = transformY;
        double round_Y = Math.Round(object_PosY,2); //맞춰야할 0.0n 위치
        float round_To_Float = (float)round_Y;
        float clamp_Round = Mathf.Clamp(round_To_Float, min_PosY, max_PosY);
        return clamp_Round;
    }

    void Play_SFX_Object(float num)
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
    void OnMouseDown()
    {
        clickPoint = Input.mousePosition;

    }

    private void OnMouseUp()
    {
        _isStop = false;
    }

    void OnMouseDrag()
    {
        Vector3 diff = Input.mousePosition - clickPoint;
        Vector3 pos = transform.localPosition;

        pos.y += diff.y * Time.deltaTime * upDownSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, Object_Cur_Pos(pos.y), transform.localPosition.z);

        if(transform.localPosition.y.Equals(Object_Cur_Pos(pos.y)) && !_isStop)
        {
            Play_SFX_Object(Object_Cur_Pos(pos.y));
        } 

        clickPoint = Input.mousePosition;
    }
}
