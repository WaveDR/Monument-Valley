using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance = null;
    [SerializeField] private Player_Controller player;
    public List<Node> targetList = new List<Node>();
    public List<Node> all_Nodes = new List<Node>();

    public Camera mainCamera;
    public Node start_Node;
    public Node end_Node;

    public int cross_Node_Length;

    public bool isClick;
    public bool isRayStart;
    public bool isRayEnd;
    public bool isRay;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        player = FindObjectOfType<Player_Controller>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        
    }
    void Update()
    {
        Input_System();

        if (!isRay)
        {
            if(isRayStart && isRayEnd)
            {
                start_Node.isNode = true;
                isRay = true;
                
            }
        }

        if (isClick)
        {
            Reset_TargetNode();

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit rayCastHit))
            {
                GameObject node_Obj = rayCastHit.collider.gameObject;
                Node node = node_Obj.GetComponent<Node>();

                if (node == null) return;
                if (node.isTarget_Node == true)
                {
                    isRay = false;
                    node.isEnd = true;
                    node.click_Pos.Play();
                    end_Node = node;
                    isRayEnd = true;
                }
            }
        }
    }
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
            //start_Node = null;
            end_Node = null;
        }
        isRayEnd = false;

    }

    public void Delete_Node_Index()
    {
        while (true)
        {
            targetList[targetList.Count -1].isDont_Able = true;
            targetList.RemoveAt(targetList.Count -1);

            if(targetList[targetList.Count -1].crossRoad)
            {
                targetList.Clear();
                foreach (Node nodes in all_Nodes)
                {
                    nodes.isPast_Node = false;
                }
                isRay = false;
                return;
            }
        }
    }

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
    
    void Input_System()
    {
        isClick = Input.GetMouseButtonDown(0);
    }
}
