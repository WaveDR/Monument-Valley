using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_Intro : MonoBehaviour
{
    [SerializeField] private float deltaTime;
    [SerializeField] private Player_Controller player;
    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if(deltaTime > 1f)
        {
            player.isMove = true;
        }
    }
}
