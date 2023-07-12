using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERotate { RotX,RotY,RotZ}
public class Rotate_Object : MonoBehaviour
{
    public ERotate rotate_Type;
    public Node[] child_Nodes;

    public Vector3 correct_Rotate;
    private Vector3 mousePos;
    private Vector3 offset;
    private Vector3 rotation;

    [SerializeField] float rotate_Speed;
    [SerializeField] Player_Controller player;

    private bool isRotate;

    // Start is called before the first frame update
    void Awake()
    {
        child_Nodes = GetComponentsInChildren<Node>();
        player = FindObjectOfType<Player_Controller>();

        foreach(Node nodes in child_Nodes)
        {
            nodes.isRotate_Node = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isRotate)
        {
            Rotate_Co(false, 0, 0, rotate_Type);
        }
        else
        {
            Auto_Rotate_Euler();
        }
    }

    private void OnMouseDown()
    {
        isRotate = true;
        mousePos = Input.mousePosition;
    }
    private void OnMouseUp()
    {
        isRotate = false;
    }
    public void Auto_Rotate_Euler()
    {
        float cur_Angle = Current_Rotate(rotate_Type);
        float target_Angle;

        //필요 각도 색인
        for (int i = 270; i >= 0 ; i-= 90)
        {
            if(cur_Angle > i)
            {
                if(cur_Angle > i+ 45)
                {
                    target_Angle= i + 90;
                }
                else
                {
                    target_Angle = i;
                }

                break;
            }
        }
    }

    IEnumerator Rotate_Co(bool auto, float start, float end, ERotate rotate_Type)
    {
        // Vector3 target = new Vector3(end, 0, 0);
        float rotation_Idx = 0;
      

        if (!auto)
        {
            offset = (Input.mousePosition - mousePos);


            rotation_Idx = -(offset.x + offset.y) * Time.deltaTime * rotate_Speed;

            transform.Rotate(rotation);

            mousePos = Input.mousePosition;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

        }

        switch (rotate_Type)
        {
            case ERotate.RotX:
                rotation.x = rotation_Idx;
                break;

            case ERotate.RotY:
                rotation.y = rotation_Idx;
                break;

            case ERotate.RotZ:
                rotation.z = rotation_Idx;

                break;
        }
    }
    public float Current_Rotate(ERotate rotate)
    {
        Quaternion current_Euler = transform.localRotation;

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
}
