using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Select_Stage : MonoBehaviour
{
    public GameObject stage_File;

    //선택한 스테이지로 이동하기
    public void Move_Scene_Level(string level)
    {
        Sound_Manager.Instance.StopAll_SFX();
        Sound_Manager.Instance.PlaySE("Select_Level");
        SceneManager.LoadScene(level);
    }

    //로비에서 게임종료 UI 호출
    public void Stage_Flie_Active(bool set_Bool)
    {
        stage_File.SetActive(set_Bool);
    }
}
