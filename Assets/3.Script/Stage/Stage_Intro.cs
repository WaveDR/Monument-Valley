using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_Intro : MonoBehaviour
{
    [SerializeField] private float deltaTime;
    [SerializeField] private Player_Controller player;

    void Awake()
    {
        player = FindObjectOfType<Player_Controller>();
    }

}
