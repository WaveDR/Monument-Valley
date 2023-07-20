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
    public bool crossRoad;     // ������ Node
    public bool stairs_Node;   // ��� Node
    public bool ladder_Node;   // ��ٸ� Node
    public bool button_Node;   // ��ư Node
    public bool goal_Node;     // �� Node

    [Header("Reverce Node")]
    [Header("=======================================")]
    public bool stairs_Node_Reverce;   // ��� Node
    public bool ladder_Node_Reverce;   // ��ٸ� Node

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
                Raycast_Node(1f, 5, false); //���� Ȯ�� ��� Node�� Vector.up�� ���� ������ Ȯ��
            else if (stairs_Node_Reverce)
                Raycast_Node(0.8f, 5, true);
            else if (ladder_Node)
                Raycast_Node(0f, 4f, false); //���� Ȯ�� ��ٸ� Node�ϵ����� [��]�� Ȯ���ϱ� ���� node Raycast ���̰� ���.
            else if (ladder_Node_Reverce)
                Raycast_Node(0f, 4f, true);
            else
                Raycast_Node(0, 1, false); //�⺻���� ��ã�⸦ ����ϴ� Node �ϵ������� Ȯ��. ���̰� ª��.

            isNode = false; //�� ������Ʈ���� �� ã�� ����
        }
    }

    //Ư��ȿ�� Node �ִϸ��̼�
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (button_Node)
            {
                if (stairs_Node_Reverce) stage_Anim.SetBool("Tower_On", true); //Ÿ�� ���� ��ư
                else if (stairs_Node) stage_Anim.SetBool("Ladder_On", true);   //�ٴ� ���� ��ư
                else stage_Anim.SetBool("Floor_Out", true);                    //��� ���� ��ư
            }
            if (goal_Node)
            {
                NodeManager.Instance.Finish_Stage();
            }
        }
    }

    //==================================================== Basic Method / CallBack Method =======================================================

    //����ĳ��Ʈ�� �ϵ����� ���� �����Ͽ� BFS ����
    public void Raycast_Node(float rayDeg, float rayLength, bool reverce)
    { 
        //�� �� �� �� �������� Raycast�� �߻��Ͽ� ���� �ν� ��θ� Ȯ�� (�����ο��� ����� ���ٸ� �� ��� isDont_Able �̳� �̹� ������ isPastNode�� �����Ͽ� Ȯ��)
        if ((Physics.Raycast(transform.position+ (  transform.forward * rayDeg) ,  transform.forward + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position+ (  transform.right   * rayDeg) ,  transform.right   + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position+ (- transform.forward * rayDeg) , -transform.forward + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position+ (- transform.right   * rayDeg) , -transform.right   + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position, Ray_Euler(1, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit)))
        {
            isTarget_Node = true; 
            isPast_Node = true; //�̹� ������ ��� ���� Node���� ���� Node�� Ȯ������ �ʵ��� ����

            //���� Ÿ�� ����� ��������
            neighbor_Node = rayHit.collider.GetComponent<Node>();
            if(neighbor_Node != null) //Null üũ
            {
                if (!neighbor_Node.isPast_Node)
                {
                    //Node Manager�� TargtList�� �߰� 
                    NodeManager.Instance.targetList.Add(gameObject.GetComponent<Node>());
                    neighbor_Node.isTarget_Node = true;

                    if (neighbor_Node.isEnd) //������
                    {
                        Debug.LogWarning("��� ���� �Ϸ�");
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
            if (isCross) //������ �������� �� �̻� ������ ������
            {
                Debug.Log("�� �̻� ���� �����ϴ�!" + gameObject.name);
                NodeManager.Instance.targetList.Clear();

                NodeManager.Instance.start_Node.Reverce_Raycast_Node(rayDeg, rayLength, reverce);
                return;
            }

            //���� ������ �Ǵ�
            Debug.Log("���� �������ϴ�! " + gameObject.name);
            isDont_Able = true;

            //�����α��� �ǵ��ư��� List���� ����
            NodeManager.Instance.Delete_Node_Index();
        }
    }
 
    //��ǥ������ ������ �ʴ´ٸ� ������������ �ݴ�������� Ȯ��
    public void Reverce_Raycast_Node(float rayDeg, float rayLength, bool reverce)
    {
        //�� �� �� �� �������� Raycast�� �߻��Ͽ� ���� �ν� ��θ� Ȯ�� (�����ο��� ����� ���ٸ� �� ��� isDont_Able �̳� �̹� ������ isPastNode�� �����Ͽ� Ȯ��)
        if ((Physics.Raycast(transform.position + (-transform.forward * rayDeg), -transform.forward + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position + (-transform.right   * rayDeg), -transform.right   + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position + ( transform.forward * rayDeg),  transform.forward + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position + ( transform.right   * rayDeg),  transform.right   + Ray_Euler(rayDeg, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit))
         || (Physics.Raycast(transform.position, Ray_Euler(1, reverce), out rayHit, range * rayLength, LayerMask.GetMask("Node")) && Node_Except(rayHit)))
        {
            isTarget_Node = true;
            isPast_Node = true; //�̹� ������ ��� ���� Node���� ���� Node�� Ȯ������ �ʵ��� ����

            //���� Ÿ�� ����� ��������
            neighbor_Node = rayHit.collider.GetComponent<Node>();
            if (neighbor_Node != null) //Null üũ
            {
                if (!neighbor_Node.isPast_Node)
                {
                    //Node Manager�� TargtList�� �߰� 
                    NodeManager.Instance.targetList.Add(gameObject.GetComponent<Node>());
                    neighbor_Node.isTarget_Node = true;

                    if (neighbor_Node.isEnd) //������
                    {
                        Debug.LogWarning("��� ���� �Ϸ�");
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
            if (isCross) //������ �������� �� �̻� ������ ������
            {
                Debug.Log("�� �̻� ���� �����ϴ�!" + gameObject.name);
                return;
            }

            //���� ������ �Ǵ�
            Debug.Log("���� �������ϴ�! " + gameObject.name);
            isDont_Able = true;

            //�����α��� �ǵ��ư��� List���� ����
            NodeManager.Instance.Delete_Node_Index();
        }
    }

    #region BFS Route Data
    public bool Node_Except(RaycastHit hit)
    {
        //Node ���� ���� ���� 
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

    //������ Draw Ray
    void Test_DrawRay(float rayDeg , float rayLength , bool reverce)
    {
        Debug.DrawRay(transform.position + ( transform.forward * rayDeg), (transform.forward + Ray_Euler(rayDeg, reverce)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + (- transform.forward * rayDeg), (-transform.forward + Ray_Euler(rayDeg, reverce)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + ( transform.right * rayDeg), (transform.right + Ray_Euler(rayDeg, reverce)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position + ( - transform.right * rayDeg), (-transform.right + Ray_Euler(rayDeg, reverce)) * range * rayLength, Color.green);
        Debug.DrawRay(transform.position, Ray_Euler(1, reverce) * range * rayLength, Color.green);
    }
}
