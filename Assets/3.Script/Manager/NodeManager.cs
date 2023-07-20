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
            //���� ���� �Ҵ�� ��ǥ ���� �Ҵ��� �Ϸ�Ǹ� ���� ��忡�� '���� �켱 Ž��' ����
            if(isRayStart && isRayEnd)
            {
                start_Node.isNode = true;
                isRay = true;
            }
        }

        //��ǥ ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            //��� Node Data �ʱ�ȭ
            Reset_TargetNode();

            //ī�޶� Raycast ����
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
    
    // ���� Scene �ȿ� �ִ� ��� Node�� �����Ͱ� �ʱ�ȭ
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

            if (nodes.crossRoad) nodes.isCross = true; //crossRoad�� Ȱ��ȭ�Ǿ��ִ� Node�� Ȱ��ȭ�Ͽ� ������ ���� �ο�
        }
        end_Node = null;
        isRayEnd = false;
    }

    //BFS�� �� Ž�� �� �� �̻� ���� ã�� �� ���� ��쿡 Cross Node���� Ƚ���� ȯ���Ͽ� Target List���� ���� 
    public void Delete_Node_Index()
    {
        player.Start_Node_Select();
        while (true)
        {
            // all_Node List���� ������ bool���� Ȯ�εǸ� ���Ÿ� ���߰� ��ã�⸦ ����
            // ó������ �ٽ� ���� ã�� isDont_Able || isPast_Node�� ������ ���� ã�´�.

            if (targetList[targetList.Count -1].isCross || targetList[targetList.Count - 1].isStart)
            {
                foreach (Node nodes in all_Nodes)
                {
                    nodes.isPast_Node = false;
                }
                targetList[targetList.Count - 1].isCross = false; // isCross�� ��Ȱ��ȭ�Ͽ� 3�ܰ�, 2�ܰ�, 1�ܰ� �����θ� ������� Ȯ��
                targetList.Clear(); //�ʱ�ȭ�Ͽ� �ٽ� ã��
                isRay = false; //Restart
                return;
            }
            else
            {
                targetList[targetList.Count - 1].isDont_Able = true; //���ڶ����� �� ��Ȯ�� �� isDont_Able�� true�� node�� ��꿡�� ����
                targetList.RemoveAt(targetList.Count - 1); //���������� �� ĭ�� ����
            }
        }
    }

    //���� �������� ��ǥ �������� ������ ��θ� �����Ͽ��� ��� Player�� TargetQueue�� �ϳ� �� ����
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

    //�÷��̾�� ȣ��. Goal �������� �������� ��� ȣ��
    public void Finish_Stage()
    {
        UI_Manager.Instance.ui_Anim.SetTrigger("Fade");
        Invoke("Goto_Lobby", 0.8f);
    }

    //Invoke�� Method
    void Goto_Lobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
