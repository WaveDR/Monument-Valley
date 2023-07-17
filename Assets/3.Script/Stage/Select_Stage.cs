using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Select_Stage : MonoBehaviour
{
    public void Move_Scene_Level(string level)
    {
        SceneManager.LoadScene(level);
    }
}
