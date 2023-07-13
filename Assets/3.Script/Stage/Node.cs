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

    [Header("Special Node")]
    [Header("=======================================")]
    public bool crossRoad;
    public bool isRotate_Node;

    [Header("Data Node")]
    [Header("=======================================")]
    public Node neighbor_Node;
    [SerializeField] private float range;
    RaycastHit rayHit;

    // Start is called before the first frame update
    void Start()
    {
        NodeManager.Instance.all_Nodes.Add(gameObject.GetComponent<Node>());
        range = 1f;
            isTarget_Node = true;
    }

    // Update is called once per frame
    void Update()
    {
        Test_DrawRay();

        if (isNode)
        {
            Raycast_Node();
            isNode = false;
        }
    }
    public void Reset_Field()
    {
        if (NodeManager.Instance.isClick)
        {
            isEnd = false;
        }
    }

    public void Raycast_Node()
    {
        if ((Physics.Raycast(transform.position, transform.forward, out rayHit, range, LayerMask.GetMask("Node"))  && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able) )
         || (Physics.Raycast(transform.position, transform.right, out rayHit, range, LayerMask.GetMask("Node"))    && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able) )
         || (Physics.Raycast(transform.position, -transform.forward, out rayHit, range, LayerMask.GetMask("Node")) && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able) )
         || (Physics.Raycast(transform.position, -transform.right, out rayHit, range, LayerMask.GetMask("Node"))   && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able) ))
        {
            isTarget_Node = true;
            isPast_Node = true;
            // 북 동 남 서 방향을 확인하여 isPast_Node의 상태 확인
            neighbor_Node = rayHit.collider.GetComponent<Node>();
            if(neighbor_Node != null)
            {
                if (!neighbor_Node.isPast_Node)
                {
                    Debug.Log(neighbor_Node.name);

                    NodeManager.Instance.targetList.Add(gameObject.GetComponent<Node>());
                    neighbor_Node.isTarget_Node = true;

                    if (neighbor_Node.isEnd) //목적지
                    {
                        Debug.Log("목적지에 도착했습니다!");
                    
                        return;
                    }

                    neighbor_Node.isNode = true; //다음 타일 로직 실행
                }
            }
        }
        else
        {
            if (crossRoad) //갈림길 기준으로 더 이상 갈곳이 없으면
            {
                Debug.Log("더 이상 길이 없습니다!" + gameObject.name);
                return;
            }

            //막힌 곳으로 판단
            Debug.Log("길이 막혔습니다! " + gameObject.name);
            isDont_Able = true;
            NodeManager.Instance.Delete_Node_Index();
        }
    }
    void Test_DrawRay()
    {
        Debug.DrawRay(transform.position, transform.forward * range, Color.green);
        Debug.DrawRay(transform.position, -transform.forward * range, Color.green);
        Debug.DrawRay(transform.position, transform.right * range, Color.green);
        Debug.DrawRay(transform.position, -transform.right * range, Color.green);
    }
}
