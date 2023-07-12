using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public Animator player_Anim;
    public float move_Speed;
    public CharacterController character_Con;
    public Vector3 movePoint;

    private RaycastHit rayHit;

    public bool isMove = false;

    // Start is called before the first frame update

    private void Awake()
    {
        TryGetComponent(out player_Anim);
        TryGetComponent(out character_Con);
    }
    void Start()
    {
        move_Speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMove) return;

        Debug.DrawRay(transform.position, -transform.up, Color.green, 3f);

        if (Physics.Raycast(transform.position, -transform.up, out rayHit, 3f, LayerMask.GetMask("Node")))
        {
            GameObject node_Obj = rayHit.collider.gameObject;
            Node node = node_Obj.GetComponent<Node>();

            if (node == null) return;
            else
            {
                node.isStart = true;
                NodeManager.Instance.isRayStart = true;
                NodeManager.Instance.start_Node = node;
            }
        }

      
           // if (Vector3.Distance(transform.position, movePoint) > 0.1f)
           // {
           //     Player_Move();
           //     player_Anim.SetBool("isMove", true);
           // }
           // else
           // {
           //     player_Anim.SetBool("isMove", false);
           //     NodeManager.Instance.isRay = false;
           // }
        
    }
    public void Player_Move()
    {
        Vector3 update_MovePoint = (movePoint - transform.position).normalized * move_Speed;

        transform.LookAt(transform.position + (movePoint - transform.position));

        update_MovePoint.y = 0;
        transform.rotation = Quaternion.LookRotation(update_MovePoint);

        character_Con.SimpleMove(update_MovePoint);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Node") || collision.gameObject.CompareTag("FirstNode"))
        {
            Node node = collision.gameObject.GetComponent<Node>();

            node.isStart = false;
        }
    }
}
