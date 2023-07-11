using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public Animator player_Anim;
    public float move_Speed;
    public CharacterController character_Con;
    public Vector3 movePoint;
    public Camera mainCamera;
    // Start is called before the first frame update

    private void Awake()
    {
        TryGetComponent(out player_Anim);
        TryGetComponent(out character_Con);
        mainCamera = Camera.main;
    }
    void Start()
    {
        move_Speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);

            if (Physics.Raycast(ray, out RaycastHit rayCastHit))
            {
                movePoint = rayCastHit.point;
               //Debug.Log("MovePoint | " + movePoint.ToString());
               //Debug.Log("¸ÂÀº °´Ã¼ | " + rayCastHit.ToString());
            }
        }
  

        if(Vector3.Distance(transform.position, movePoint) > 0.1f)
        {
            Player_Move();
            player_Anim.SetBool("isMove", true);
        }
        else
        {
            player_Anim.SetBool("isMove", false);
        }
    }
    public void Player_Move()
    {
        Vector3 update_MovePoint = (movePoint - transform.position).normalized * move_Speed;

        transform.LookAt(transform.position + (movePoint - transform.position));

        update_MovePoint.y = 0;
        transform.rotation = Quaternion.LookRotation(update_MovePoint);

        character_Con.SimpleMove(update_MovePoint);
    }
}
