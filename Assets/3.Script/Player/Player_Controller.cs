using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player_Controller : MonoBehaviour
{
    public Animator player_Anim;
    public float move_Speed;
    public GameObject node_Obj;

    public bool isMove = false;

    public Queue<Node> move_Tatget_Queue = new Queue<Node>();
    public List<Node> move_Target_List = new List<Node>();
    RaycastHit hit;
    // Start is called before the first frame update

    private void Awake()
    {
        TryGetComponent(out player_Anim);
    }
    void Start()
    {
        move_Speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        move_Target_List = move_Tatget_Queue.ToList();

        if (!isMove)
        {
            player_Anim.SetBool("isMove", false);
        }

        if(move_Tatget_Queue.Count >0)
        {
            Player_Move();
            player_Anim.SetBool("isMove", true);
        }

        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.green);

    }
    public void Player_Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, move_Tatget_Queue.Peek().transform.position, move_Speed * Time.deltaTime);

        if(move_Tatget_Queue.Peek().ladder_Node)
            player_Anim.SetBool("isLadder", true);
        else
            player_Anim.SetBool("isLadder", false);

     

        if (transform.position == move_Tatget_Queue.Peek().transform.position)
        {
            move_Tatget_Queue.Dequeue();
        }
        if (move_Tatget_Queue.Count == 0)
        {
            isMove = false;
            return;
        }

        transform.LookAt(move_Tatget_Queue.Peek().transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
        //character_Con.SimpleMove(update_MovePoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Node"))
        {
            Debug.Log("인식시작");
            Node node = other.GetComponent<Node>();
            node_Obj = node.gameObject;
            if (node == null) return;
            else
            {
                NodeManager.Instance.start_Node = node;
                NodeManager.Instance.isRayStart = true;
                node.isStart = true;
            }
            if (node.stairs_Node)
            {
                transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
        }
        if (other.CompareTag("Rotator"))
        {
            Rotate_Object rotator = other.GetComponent<Rotate_Object>();
            rotator.rotator_Anim.SetBool("Player_On", true);
            rotator.isControl = false;
        }
        if (other.CompareTag("Fillor"))
        {
            transform.SetParent(other.transform);
            //todo 드래그 오브젝트 위로 올라갓을 경우 캐릭터 부모오브젝트 변경
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Node"))
        {
            Node node = other.GetComponent<Node>();
            node.isStart = false;
        }

        if (other.CompareTag("Rotator"))
        {
            Rotate_Object rotator = other.GetComponent<Rotate_Object>();
            rotator.rotator_Anim.SetBool("Player_On", false);
            rotator.isControl = true;
        }

        if (other.CompareTag("Fillor"))
        {
            transform.SetParent(null);
            //todo 드래그 오브젝트 위로 올라갓을 경우 캐릭터 부모오브젝트 변경
        }

    }
}
