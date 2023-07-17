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
    

    float Object_Cur_Pos(float transformY)
    {
        float object_PosY = transformY;
        double round_Y = Math.Round(object_PosY,2); //맞춰야할 0.0n 위치

        float round_To_Float = (float)round_Y;

       float clamp_Round = Mathf.Clamp(round_To_Float, min_PosY, max_PosY);

        Debug.Log(clamp_Round);

        return clamp_Round;
    }
    void OnMouseDown()
    {
        clickPoint = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 diff = Input.mousePosition - clickPoint;
        Vector3 pos = transform.localPosition;

        pos.y += diff.y * Time.deltaTime * upDownSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, Object_Cur_Pos(pos.y), transform.localPosition.z);
        //transform.position = pos;

        clickPoint = Input.mousePosition;
    }
}
