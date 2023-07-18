using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager Instance = null;
    public Animator ui_Anim;

    [SerializeField] private Text title_Level_Text;
    [SerializeField] private Text title_Name_Text;
    [SerializeField] private Text title_Detail_Text;

    [SerializeField] private string[] title_Level_Str;
    [SerializeField] private string[] title_Name_Str;
    [SerializeField] private string[] title_Detail_Str;
    [SerializeField] private GameObject title_Obejct;

    private Scene cur_Scene;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        TryGetComponent(out ui_Anim);
        cur_Scene = SceneManager.GetActiveScene();
    }
    private void Start()
    {
  
        if (cur_Scene.name.Equals("Lobby"))
        {
            return;
        }

        //이 부분은 DB로 처리
        else if (cur_Scene.name.Equals("Stage01"))
        {
            title_Level_Text.text = title_Level_Str[0];
            title_Name_Text.text = title_Name_Str[0];
            title_Detail_Text.text = title_Detail_Str[0];
        }
        else
        {
            title_Level_Text.text = title_Level_Str[1];
            title_Name_Text.text = title_Name_Str[1];
            title_Detail_Text.text = title_Detail_Str[1];
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ui_Anim.SetBool("GameStart", true);
            title_Obejct.SetActive(false);
        }
    }
    public void Set_UI(bool set_Bool)
    {
        ui_Anim.SetBool("Setting_UI", set_Bool);
    }

    public void Goto_Lobby(bool set_Bool)
    {
        ui_Anim.SetBool("Exit", set_Bool);

    }

    public void Restart_Stage()
    {
        ui_Anim.SetTrigger("Fade");
        Invoke("Restart_Scene", 0.5f);
    }

    void Restart_Scene()
    {
        SceneManager.LoadScene(cur_Scene.name);
    }
}
