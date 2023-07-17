using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager Instance = null;
    public Animator ui_Anim;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        TryGetComponent(out ui_Anim);
    }

    public void Set_UI(bool set_Bool)
    {
        ui_Anim.SetBool("Setting_UI", set_Bool);
    }

    public void Restart_Stage()
    {
        ui_Anim.SetTrigger("Fade");
        Invoke("Restart_Scene", 0.5f);
    }

    void Restart_Scene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
