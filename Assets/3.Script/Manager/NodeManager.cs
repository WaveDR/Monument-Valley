using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance = null;
    [SerializeField] private Player_Controller player;

    [Header("BFS Data")]
    [Header("=======================================")]
    public Camera mainCamera;

    public List<Node> targetList = new List<Node>();
    public List<Node> all_Nodes = new List<Node>();

    public Node start_Node;
    public Node end_Node;

    public int cross_Node_Length;

    public bool isRayStart;
    public bool isRayEnd;
    public bool isRay;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        player = FindObjectOfType<Player_Controller>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!isRay)
        {
            //시작 지점 할당과 목표 지점 할당이 완료되면 시작 노드에서 '깊이 우선 탐색' 시작
            if(isRayStart && isRayEnd)
            {
                start_Node.isNode = true;
                isRay = true;
            }
        }

        //목표 지점 선택
        if (Input.GetMouseButtonDown(0))
        {
            //모든 Node Data 초기화
            Reset_TargetNode();

            //카메라 Raycast 실행
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit rayCastHit))
            {
                GameObject node_Obj = rayCastHit.collider.gameObject;
                Node node = node_Obj.GetComponent<Node>();

                if (node == null) return;
                if (node.isTarget_Node == true)
                {
                    Sound_Manager.Instance.StopAll_SFX();
                    Sound_Manager.Instance.PlaySE("Move_Point");
                    isRay = false;
                    node.isEnd = true;
                    node.click_Pos.Play();
                    end_Node = node;
                    isRayEnd = true;
                }
            }
        }
    }

    //==================================================== Basic Method / CallBack Method =======================================================
    
    // 현재 Scene 안에 있는 모든 Node의 데이터값 초기화
    public void Reset_TargetNode()
    {
        targetList.Clear();
        player.move_Tatget_Queue.Clear();

        foreach (Node nodes in all_Nodes)
        {
            nodes.isPast_Node = false;
            nodes.isDont_Able = false;
            nodes.isStart = false;
            nodes.isEnd = false;

            if (nodes.crossRoad) nodes.isCross = true; //crossRoad가 활성화되어있는 Node는 활성화하여 교차로 역할 부여
        }
        end_Node = null;
        isRayEnd = false;
    }

    //BFS로 길 탐색 중 더 이상 길을 찾을 수 없을 경우에 Cross Node까지 횟수를 환산하여 Target List에서 제외 
    public void Delete_Node_Index()
    {
        player.Start_Node_Select();
        while (true)
        {
            // all_Node List에서 교차로 bool값이 확인되면 제거를 멈추고 길찾기를 리셋
            // 처음부터 다시 길을 찾되 isDont_Able || isPast_Node를 제외한 길을 찾는다.

            if (targetList[targetList.Count -1].isCross || targetList[targetList.Count - 1].isStart)
            {
                foreach (Node nodes in all_Nodes)
                {
                    nodes.isPast_Node = false;
                }
                targetList[targetList.Count - 1].isCross = false; // isCross를 비활성화하여 3단계, 2단계, 1단계 교차로를 순서대로 확인
                targetList.Clear(); //초기화하여 다시 찾기
                isRay = false; //Restart
                return;
            }
            else
            {
                targetList[targetList.Count - 1].isDont_Able = true; //끝자락부터 길 재확인 중 isDont_Able이 true인 node는 계산에서 제외
                targetList.RemoveAt(targetList.Count - 1); //마지막에서 한 칸씩 제거
            }
        }
    }

    //시작 지점부터 목표 지점까지 최적의 경로를 검출하였을 경우 Player의 TargetQueue에 하나 씩 전송
    public void Export_Target_List()
    {
        while (true)
        {
            player.move_Tatget_Queue.Enqueue(targetList[0]);
            targetList.RemoveAt(0);
            if (targetList.Count <= 0)
            {
                player.isMove = true;
                return;
            }
        }
    }

    //플레이어에서 호출. Goal 지점까지 도착했을 경우 호출
    public void Finish_Stage()
    {
        UI_Manager.Instance.ui_Anim.SetTrigger("Fade");
        Invoke("Goto_Lobby", 0.8f);
    }

    //Invoke용 Method
    void Goto_Lobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
