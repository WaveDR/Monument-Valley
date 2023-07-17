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
    public bool rotate_Node;
    public bool stairs_Node;
    public bool ladder_Node;
    public bool button_Node;

    [Header("Data Node")]
    [Header("=======================================")]
    public Node neighbor_Node;
    [SerializeField] private float range;
    [SerializeField] private int stage_Anim_Num;
    public ParticleSystem click_Pos;
    public Animator stage_Anim;
    RaycastHit rayHit;

    // Start is called before the first frame update
    void Start()
    {
        NodeManager.Instance.all_Nodes.Add(gameObject.GetComponent<Node>());
        stage_Anim = GetComponentInParent<Animator>();
        range = 1f;
        isTarget_Node = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (stairs_Node)
            Test_DrawRay(1f, 5, ladder_Node);
        else if (ladder_Node)
            Test_DrawRay(0, 5, ladder_Node);
        else
            Test_DrawRay(0, 1, ladder_Node);

        if (isNode)
        {
            if (stairs_Node)
                Raycast_Node(1f, 5);
            else if (ladder_Node)
                Raycast_Node(0f, 5f);
            else
                Raycast_Node(0, 1);

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

    public void Raycast_Node(float rayDeg, float rayLength)
    { 
        if ((Physics.Raycast(transform.position+ (  transform.forward * rayDeg) ,  transform.forward + (Vector3.up * rayDeg), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able))
         || (Physics.Raycast(transform.position+ (  transform.right * rayDeg)   ,  transform.right   + (Vector3.up * rayDeg), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able))
         || (Physics.Raycast(transform.position+ (- transform.forward * rayDeg) , -transform.forward + (Vector3.up * rayDeg), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able))
         || (Physics.Raycast(transform.position+ (- transform.right * rayDeg)   , -transform.right   + (Vector3.up * rayDeg), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able))
         || (Physics.Raycast(transform.position,  transform.up, out rayHit, range * rayLength, LayerMask.GetMask("Node"))  && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able)))
        {
            isTarget_Node = true;
            isPast_Node = true;
            //북 동 남 서 방향을 확인하여 isPast_Node의 상태 확인
            neighbor_Node = rayHit.collider.GetComponent<Node>();
            if(neighbor_Node != null)
            {
                if (!neighbor_Node.isPast_Node)
                {
                   // Debug.Log(neighbor_Node.name);

                    NodeManager.Instance.targetList.Add(gameObject.GetComponent<Node>());
                    neighbor_Node.isTarget_Node = true;

                    if (neighbor_Node.isEnd) //목적지
                    {
                        Debug.LogWarning("목적지에 도착했습니다!");
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
    void Test_DrawRay(float rayDeg , float rayLength , bool ladder)
    {
        Debug.DrawRay(transform.position + ( transform.forward * rayDeg), (transform.forward + (Vector3.up * rayDeg)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + (- transform.forward * rayDeg), (-transform.forward + (Vector3.up * rayDeg)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + ( transform.right * rayDeg), (transform.right + (Vector3.up * rayDeg)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + ( - transform.right * rayDeg), (-transform.right + (Vector3.up * rayDeg)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position, transform.up * range * rayLength, Color.green);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (button_Node)
            {
                stage_Anim.SetBool("Floor_Out", true);

                if(stairs_Node) stage_Anim.SetBool("Ladder_On", true);
            }
        }
    }
}
