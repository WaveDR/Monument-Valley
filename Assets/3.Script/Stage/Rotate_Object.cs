using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERotate { RotX, RotY, RotZ }
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

    private bool isCurrect;
    public bool isControl = true;

    private bool _isStop;
    public Animator rotator_Anim;

    public GameObject light_Object;

    // Start is called before the first frame update
    void Awake()
    {
        child_Nodes = GetComponentsInChildren<Node>();
        player = FindObjectOfType<Player_Controller>();
        TryGetComponent(out rotator_Anim);
        foreach (Node nodes in child_Nodes)
        {
            nodes.rotate_Node = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isControl) return;
        if (isRotate)
        {
            _isStop = false;
            offset = (Input.mousePosition - mousePos);
            isCurrect = false;

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


        else
        {
            if (!isCurrect)
                Auto_Rotate_Euler();

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

    private void OnMouseDown()
    {
        isRotate = true;
        mousePos = Input.mousePosition;
    }
    private void OnMouseUp()
    {
        isRotate = false;
    }
    //private void OnMouseOver()
    //{
    //    light_Object.SetActive(true);
    //    light_Object.transform.localPosition = Input.mousePosition * Time.deltaTime * 5;
    //}
    //private void OnMouseExit()
    //{
    //    light_Object.SetActive(false);
    //}
    public void Auto_Rotate_Euler()
    {
        float cur_Angle = Current_Rotate(rotate_Type);
        float target_Angle;

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
                Auto_Euler(cur_Angle, target_Angle);
            }
        }
    }

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

        if ((int)transform.localRotation.x != (int)end)
            transform.localEulerAngles = new Vector3(start + Time.deltaTime * rotate_Speed, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        else
        {
            transform.localEulerAngles = new Vector3(start - Time.deltaTime * rotate_Speed, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        }

        if (transform.localEulerAngles.x >= end - 0.3f && transform.localEulerAngles.x <= end + 0.3f) isCurrect = true;

    }
}