using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Select_Stage : MonoBehaviour
{
    public GameObject stage_File;

    //������ ���������� �̵��ϱ�
    public void Move_Scene_Level(string level)
    {
        Sound_Manager.Instance.StopAll_SFX();
        Sound_Manager.Instance.PlaySE("Select_Level");
        SceneManager.LoadScene(level);
    }

    //�κ񿡼� �������� UI ȣ��
    public void Stage_Flie_Active(bool set_Bool)
    {
        stage_File.SetActive(set_Bool);
    }
}
