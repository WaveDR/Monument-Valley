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
    public Node ableSet_Node;
    public Node start_Node;
    public Node end_Node;

    public int cross_Node_Length;

    public bool isClick;
    public bool isRayStart;
    public bool isRayEnd;

    private bool isScaning;
    private bool isRay;
    private int _scanNum;

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
        ableSet_Node = GameObject.FindGameObjectWithTag("FirstNode").GetComponent<Node>();
        mainCamera = Camera.main;
    }
    private void Start()
    {
        ableSet_Node.isNode = true;
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
            foreach (Node nodes in all_Nodes)
            {
                nodes.Reset_Field();
            }
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);

            if (Physics.Raycast(ray, out RaycastHit rayCastHit))
            {
                GameObject node_Obj = rayCastHit.collider.gameObject;
                Node node = node_Obj.GetComponent<Node>();

                if (node == null) return;
                if (node.isTarget_Node == true)
                {
                    isRay = false;
                    node.isEnd = true;
                    end_Node = node;
                    isRayEnd = true;
                    //movePoint = rayCastHit.point;
                }
                //Debug.Log("MovePoint | " + movePoint.ToString());
                //Debug.Log("¸ÂÀº °´Ã¼ | " + rayCastHit.ToString());
            }

        }
    }
    public void Reset_TargetNode()
    {
        targetList.Clear();
        foreach (Node nodes in all_Nodes)
        {
            nodes.isDont_Able = false;
            nodes.isPast_Node = false;
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

    
    void Input_System()
    {
        isClick = Input.GetMouseButtonDown(0);
    }
}
