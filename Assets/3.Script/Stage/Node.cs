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

        if (!stairs_Node)
            Test_DrawRay(0, 1);
        else
            Test_DrawRay(1f, 5);

        if (isNode)
        {
            if (!stairs_Node)
              Raycast_Node(0, 1);
            else
              Raycast_Node(1f, 5);

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

    public void Raycast_Node(float rayDeg, int rayLength)
    { 
        if ((Physics.Raycast(transform.position+ (transform.forward * rayDeg / 2)   , transform.forward + (Vector3.up * rayDeg), out rayHit, range * rayLength, LayerMask.GetMask("Node"))  && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able) )
         || (Physics.Raycast(transform.position+ (transform.right * rayDeg /2)      , transform.right + (Vector3.up * rayDeg), out rayHit, range * rayLength, LayerMask.GetMask("Node"))    && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able) )
         || (Physics.Raycast(transform.position+ (- transform.forward * rayDeg / 2) , -transform.forward + (Vector3.up * rayDeg), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able) )
         || (Physics.Raycast(transform.position+ (-transform.right * rayDeg / 2)    , -transform.right + (Vector3.up * rayDeg), out rayHit, range * rayLength, LayerMask.GetMask("Node"))   && (!rayHit.collider.GetComponent<Node>().isPast_Node && !rayHit.collider.GetComponent<Node>().isDont_Able) ))
        {
            isTarget_Node = true;
            isPast_Node = true;
            // �� �� �� �� ������ Ȯ���Ͽ� isPast_Node�� ���� Ȯ��
            if(!stairs_Node)
            neighbor_Node = rayHit.collider.GetComponent<Node>();
            if(neighbor_Node != null)
            {
                if (!neighbor_Node.isPast_Node)
                {
                   // Debug.Log(neighbor_Node.name);

                    NodeManager.Instance.targetList.Add(gameObject.GetComponent<Node>());
                    neighbor_Node.isTarget_Node = true;

                    if (neighbor_Node.isEnd) //������
                    {
                        Debug.LogWarning("�������� �����߽��ϴ�!");
                        NodeManager.Instance.targetList.Add(neighbor_Node);
                        NodeManager.Instance.Export_Target_List();
                        return;
                    }

                    neighbor_Node.isNode = true; //���� Ÿ�� ���� ����
                }
            }
        }
        else
        {
            if (crossRoad) //������ �������� �� �̻� ������ ������
            {
                Debug.Log("�� �̻� ���� �����ϴ�!" + gameObject.name);
                return;
            }

            //���� ������ �Ǵ�
            Debug.Log("���� �������ϴ�! " + gameObject.name);
            isDont_Able = true;
            NodeManager.Instance.Delete_Node_Index();
        }
    }
    void Test_DrawRay(float rayDeg ,int rayLength)
    {
        Debug.DrawRay(transform.position + (transform.forward * rayDeg / 2), (transform.forward + (Vector3.up * rayDeg ) )* range * rayLength, Color.green);
        Debug.DrawRay(transform.position + (-transform.forward * rayDeg / 2), (-transform.forward + (Vector3.up * rayDeg)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + (transform.right * rayDeg / 2), (transform.right + (Vector3.up * rayDeg)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + (-transform.right * rayDeg / 2), (-transform.right + (Vector3.up * rayDeg)) * range * rayLength, Color.green);
    }
}
