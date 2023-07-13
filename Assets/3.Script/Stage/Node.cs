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
            // �� �� �� �� ������ Ȯ���Ͽ� isPast_Node�� ���� Ȯ��
            neighbor_Node = rayHit.collider.GetComponent<Node>();
            if(neighbor_Node != null)
            {
                if (!neighbor_Node.isPast_Node)
                {
                    Debug.Log(neighbor_Node.name);

                    NodeManager.Instance.targetList.Add(gameObject.GetComponent<Node>());
                    neighbor_Node.isTarget_Node = true;

                    if (neighbor_Node.isEnd) //������
                    {
                        Debug.Log("�������� �����߽��ϴ�!");
                    
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
    void Test_DrawRay()
    {
        Debug.DrawRay(transform.position, transform.forward * range, Color.green);
        Debug.DrawRay(transform.position, -transform.forward * range, Color.green);
        Debug.DrawRay(transform.position, transform.right * range, Color.green);
        Debug.DrawRay(transform.position, -transform.right * range, Color.green);
    }
}
