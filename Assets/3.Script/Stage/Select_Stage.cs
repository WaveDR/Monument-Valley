using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Select_Stage : MonoBehaviour
{
    public GameObject stage_File;
    public void Move_Scene_Level(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void Stage_Flie_Active(bool set_Bool)
    {
        stage_File.SetActive(set_Bool);
    }
}
