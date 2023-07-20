using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Basic Node")]
    [Header("=======================================")]
    public bool isTarget_Node;
    public bool isStart;
    public bool isEnd;

    [Header("Don't Move Node")]
    [Header("=======================================")]
    public bool isPast_Node;
    public bool isDont_Able;
    public bool isNode;
    public bool isCross;

    [Header("Special Node")]
    [Header("=======================================")]
    public bool crossRoad;     // 교차로 Node
    public bool stairs_Node;   // 계단 Node
    public bool ladder_Node;   // 사다리 Node
    public bool button_Node;   // 버튼 Node
    public bool goal_Node;     // 골 Node

    [Header("Reverce Node")]
    [Header("=======================================")]
    public bool stairs_Node_Reverce;   // 계단 Node
    public bool ladder_Node_Reverce;   // 사다리 Node

    [Header("Data Node")]
    [Header("=======================================")]
    public Node neighbor_Node;
    [SerializeField] private float range;
    [SerializeField] private int stage_Anim_Num;
    public ParticleSystem click_Pos;
    public Animator stage_Anim;
    RaycastHit rayHit;

    void Start()
    {
        NodeManager.Instance.all_Nodes.Add(gameObject.GetComponent<Node>());
        stage_Anim = GetComponentInParent<Animator>();
        range = 1f;
        isTarget_Node = true;
    }

    void Update()
    {

        //Draw Ray
        if (stairs_Node)
            Test_DrawRay(1f, 5, false);

        else if (stairs_Node_Reverce)
            Test_DrawRay(0.8f, 5, true);

        else if (ladder_Node)
            Test_DrawRay(0, 4, false);

        else if (ladder_Node_Reverce)
            Test_DrawRay(0, 4, true);

        else
            Test_DrawRay(0, 1, false);

        //BFS Start
        if (isNode)
        {
            if (stairs_Node)
                Raycast_Node(1f, 5, false); //각도 확인 계단 Node는 Vector.up을 곱한 각도로 확인
            else if (stairs_Node_Reverce)
                Raycast_Node(0.8f, 5, true);
            else if (ladder_Node)
                Raycast_Node(0f, 4f, false); //각도 확인 사다리 Node북동남서 [상]을 확인하기 위한 node Raycast 길이가 길다.
            else if (ladder_Node_Reverce)
                Raycast_Node(0f, 4f, true);
            else
                Raycast_Node(0, 1, false); //기본적인 길찾기를 담당하는 Node 북동남서를 확인. 길이가 짧다.

            isNode = false; //이 오브젝트에서 길 찾기 종료
        }
    }

    //특수효과 Node 애니메이션
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (button_Node)
            {
                if (stairs_Node_Reverce) stage_Anim.SetBool("Tower_On", true); //타워 생성 버튼
                else if (stairs_Node) stage_Anim.SetBool("Ladder_On", true);   //바닥 삭제 버튼
                else stage_Anim.SetBool("Floor_Out", true);                    //계단 생성 버튼
            }
            if (goal_Node)
            {
                NodeManager.Instance.Finish_Stage();
            }
        }
    }

    //==================================================== Basic Method / CallBack Method =======================================================

    //레이캐스트로 북동남서 상하 연결하여 BFS 구현
    public void Raycast_Node(float rayDeg, float rayLength, bool reverce)
    { 
        //북 동 남 서 방향으로 Raycast를 발사하여 다음 인식 경로를 확인 (교차로에서 검출된 막다른 길 경로 isDont_Able 이나 이미 지나온 isPastNode는 제외하여 확인)
        if ((Physics.Raycast(transform.position+ (  transform.forward * rayDeg) ,  transform.forward + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position+ (  transform.right   * rayDeg) ,  transform.right   + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position+ (- transform.forward * rayDeg) , -transform.forward + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position+ (- transform.right   * rayDeg) , -transform.right   + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position, Ray_Euler(1, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit)))
        {
            isTarget_Node = true; 
            isPast_Node = true; //이미 지나온 길로 다음 Node에서 이전 Node를 확인하지 않도록 수정

            //다음 타일 제어권 가져오기
            neighbor_Node = rayHit.collider.GetComponent<Node>();
            if(neighbor_Node != null) //Null 체크
            {
                if (!neighbor_Node.isPast_Node)
                {
                    //Node Manager의 TargtList에 추가 
                    NodeManager.Instance.targetList.Add(gameObject.GetComponent<Node>());
                    neighbor_Node.isTarget_Node = true;

                    if (neighbor_Node.isEnd) //목적지
                    {
                        Debug.LogWarning("경로 구성 완료");
                        NodeManager.Instance.targetList.Add(neighbor_Node);
                        NodeManager.Instance.Export_Target_List();
                        return;
                    }

                    neighbor_Node.isNode = true; //다음 타일 로직 실행
                }
            }
        }
        else
        {
            if (isCross) //갈림길 기준으로 더 이상 갈곳이 없으면
            {
                Debug.Log("더 이상 길이 없습니다!" + gameObject.name);
                NodeManager.Instance.targetList.Clear();

                NodeManager.Instance.start_Node.Reverce_Raycast_Node(rayDeg, rayLength, reverce);
                return;
            }

            //막힌 곳으로 판단
            Debug.Log("길이 막혔습니다! " + gameObject.name);
            isDont_Able = true;

            //교차로까지 되돌아가며 List에서 제거
            NodeManager.Instance.Delete_Node_Index();
        }
    }
 
    //목표지점이 보이지 않는다면 시작지점에서 반대방향으로 확인
    public void Reverce_Raycast_Node(float rayDeg, float rayLength, bool reverce)
    {
        //남 서 북 동 방향으로 Raycast를 발사하여 다음 인식 경로를 확인 (교차로에서 검출된 막다른 길 경로 isDont_Able 이나 이미 지나온 isPastNode는 제외하여 확인)
        if ((Physics.Raycast(transform.position + (-transform.forward * rayDeg), -transform.forward + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position + (-transform.right   * rayDeg), -transform.right   + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position + ( transform.forward * rayDeg),  transform.forward + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position + ( transform.right   * rayDeg),  transform.right   + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position, Ray_Euler(1, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit)))
        {
            isTarget_Node = true;
            isPast_Node = true; //이미 지나온 길로 다음 Node에서 이전 Node를 확인하지 않도록 수정

            //다음 타일 제어권 가져오기
            neighbor_Node = rayHit.collider.GetComponent<Node>();
            if (neighbor_Node != null) //Null 체크
            {
                if (!neighbor_Node.isPast_Node)
                {
                    //Node Manager의 TargtList에 추가 
                    NodeManager.Instance.targetList.Add(gameObject.GetComponent<Node>());
                    neighbor_Node.isTarget_Node = true;

                    if (neighbor_Node.isEnd) //목적지
                    {
                        Debug.LogWarning("경로 구성 완료");
                        NodeManager.Instance.targetList.Add(neighbor_Node);
                        NodeManager.Instance.Export_Target_List();
                        return;
                    }

                    neighbor_Node.isNode = true; //다음 타일 로직 실행
                }
            }
        }
        else
        {
            if (isCross) //갈림길 기준으로 더 이상 갈곳이 없으면
            {
                Debug.Log("더 이상 길이 없습니다!" + gameObject.name);
                return;
            }

            //막힌 곳으로 판단
            Debug.Log("길이 막혔습니다! " + gameObject.name);
            isDont_Able = true;

            //교차로까지 되돌아가며 List에서 제거
            NodeManager.Instance.Delete_Node_Index();
        }
    }

    #region BFS Route Data
    public bool Node_Except(RaycastHit hit)
    {
        //Node 제외 조건 찹조 
        if (!hit.collider.GetComponent<Node>().isPast_Node && !hit.collider.GetComponent<Node>().isDont_Able)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Vector3 Ray_Euler(float rayDeg, bool reverce)
    {
        if (!reverce)
        {
            return Vector3.up * rayDeg;
        }
        else
        {
            return Vector3.down * rayDeg;
        }
    }

    #endregion

    //기즈모용 Draw Ray
    void Test_DrawRay(float rayDeg , float rayLength , bool reverce)
    {
        Debug.DrawRay(transform.position + ( transform.forward * rayDeg), (transform.forward + Ray_Euler(rayDeg, reverce)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + (- transform.forward * rayDeg), (-transform.forward + Ray_Euler(rayDeg, reverce)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + ( transform.right * rayDeg), (transform.right + Ray_Euler(rayDeg, reverce)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + ( - transform.right * rayDeg), (-transform.right + Ray_Euler(rayDeg, reverce)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position, Ray_Euler(1, reverce) * range * rayLength, Color.green);
    }
}
