using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Player Data")]
    [Header("=======================================")]
    public Animator player_Anim;
    public float move_Speed;
    public bool isMove = false;
    private bool _isCurve;

    [Header("Target Node")]
    [Header("=======================================")]
    public Queue<Node> move_Tatget_Queue = new Queue<Node>();

    [Header("Current Node")]
    [Header("=======================================")]
    public GameObject node_Obj;


    //사다리 탈 시 회전 버그 수정 및 켜질 때 애니메이션 or UI 추가하기 (내일)
    private void Awake()
    {
        TryGetComponent(out player_Anim);
        move_Speed = 2.0f;
    }

    void Update()
    {

        if (!isMove)
        {
            player_Anim.SetBool("isMove", false);
        }

        // 목표 Queue가 1개 이상일 경우
        if(move_Tatget_Queue.Count >0)
        {
            Player_Move();
            player_Anim.SetBool("isMove", true);
        }
    }

    public void Start_Node_Select()
    {
        Node node = node_Obj.GetComponent<Node>();
        //NodeManager와 대상 Node에 시작 지점 위치 값과 조건 전송
        NodeManager.Instance.start_Node = node;
        NodeManager.Instance.isRayStart = true;
        node.isStart = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Trigger로 현재 위치에 있는 Node를 시작 Node로 업데이트
        if (other.CompareTag("Node"))
        {
            Node node = other.GetComponent<Node>();

            //시작 Node 갱신
            node_Obj = node.gameObject;

            //Null 체크
            if (node == null) return;
            else
            {
                Start_Node_Select();
            }
        }

        // 회전 오브젝트 기능 정지
        if (other.CompareTag("Rotator"))
        {
            Rotate_Object rotator = other.GetComponent<Rotate_Object>();
            rotator.rotator_Anim.SetBool("Player_On", true);
            rotator.isControl = false;
        }

        // 드래그 오브젝트 위로 올라갔을 경우 캐릭터의 부모 오브젝트 변경
        if (other.CompareTag("Fillor"))
        {
            transform.SetParent(other.transform);
        }
    }

    // Trigger 상호작용 오브젝트 해제
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Node"))
        {
            //시작지점 초기화
            Node node = other.GetComponent<Node>();
            node.isStart = false;
        }

        //회전 오브젝트 기능 재개
        if (other.CompareTag("Rotator"))
        {
            Rotate_Object rotator = other.GetComponent<Rotate_Object>();
            rotator.rotator_Anim.SetBool("Player_On", false);
            rotator.isControl = true;
        }

        //부모 오브젝트 해제
        if (other.CompareTag("Fillor"))
        {
            transform.SetParent(null);
        }
    }

    //==================================================== Basic Method / CallBack Method =======================================================

    public void Player_Move()
    {
        transform.LookAt(move_Tatget_Queue.Peek().transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0)); //플레이어 회전축을 y만 움직이게 고정

        //다음 Queue로 캐릭터 위치 변경
        transform.position = Vector3.MoveTowards(transform.position, move_Tatget_Queue.Peek().transform.position, move_Speed * Time.deltaTime);

        //목표 지점 도착 시 다음 Queue 갱신
        if (transform.position == move_Tatget_Queue.Peek().transform.position)
        {
            //현재 queue가 사다리일 경우 애니메이션 변경

            if (move_Tatget_Queue.Peek().ladder_Node || move_Tatget_Queue.Peek().ladder_Node_Reverce)
            {
                player_Anim.SetBool("isLadder", true);
            }

            if (!move_Tatget_Queue.Peek().ladder_Node && !move_Tatget_Queue.Peek().ladder_Node_Reverce)
            {
                player_Anim.SetBool("isLadder", false);
            }
               
            move_Tatget_Queue.Dequeue();

        }


        //잔여 목표 queue가 남지 않을 경우 애니메이션 변경 후 return;
        if (move_Tatget_Queue.Count == 0)
        {
            isMove = false;
            return;
        }
    }
}
