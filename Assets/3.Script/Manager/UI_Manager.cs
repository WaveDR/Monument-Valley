using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;
using UnityEngine.Audio;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance = null;

    [Header("UI")]
    [Header("=======================================")]
    [SerializeField] private Text title_Level_Text;
    [SerializeField] private Text title_Name_Text;
    [SerializeField] private Text title_Detail_Text;
    [SerializeField] private Text player_Name;

    [Header("UI Page")]
    [Header("=======================================")]
    [SerializeField] private GameObject title_Obejct;
    [SerializeField] private GameObject setting_Base;
    [SerializeField] private GameObject option;

    [Header("UI DB")]
    [Header("=======================================")]
    [SerializeField] private string title_Path;

    [Header("UI Audio")]
    [Header("=======================================")]
    public AudioMixer mixer;
    public Slider bgm_Slider;
    public Slider sfx_Slider;

    [Header("ETC")]
    [Header("=======================================")]
    public Animator ui_Anim;
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

        //현재 Scene 이름 가져오기
        cur_Scene = SceneManager.GetActiveScene();

        //Player 닉네임을 Sql에서 가져오기
        player_Name.text = SQL_Manager.Instance.info.u_Name;

        //DB 경로 추적
        title_Path = Application.dataPath + "/DataBase";

        //ETC Reset
        setting_Base.SetActive(false);
        bgm_Slider.value = 0.5f;
        sfx_Slider.value = 0.5f;
    }
    private void Start()
    {
        //현재 Scene이 Lobby면 Return;
        if (cur_Scene.name.Equals("Lobby"))
        {
            title_Obejct.SetActive(false);
            return;
        }

        //Json DB로 타이틀 대본 적용
        else if (cur_Scene.name.Equals("Stage01"))
        {
            Title_Text(title_Path, 0); // DB경로, Stage 레벨
        }
        else
        {
            Title_Text(title_Path, 1); // DB경로, Stage 레벨
        }
    }

    private void Update()
    {
        //Stage일 경우 클릭으로 다음 넘어가기
        if (Input.GetMouseButtonDown(0))
        {
            ui_Anim.SetBool("GameStart", true);
            title_Obejct.SetActive(false);
        }
    }


    //==================================================== Basic Method / CallBack Method =======================================================


    // 상단 UI 호출
    public void Set_UI(bool set_Bool)
    {
        ui_Anim.SetBool("Setting_UI", set_Bool);

        // Sound

        Sound_Manager.Instance.StopAll_SFX();
        if(set_Bool)
            Sound_Manager.Instance.PlaySE("UI_On");
        else
            Sound_Manager.Instance.PlaySE("UI_Off");
    }

    // 나가기 UI 호출
    public void Spawn_Lobby_UI(bool set_Bool)
    {
        ui_Anim.SetBool("Exit", set_Bool);


        // Sound

        Sound_Manager.Instance.StopAll_SFX();
        if (set_Bool)
            Sound_Manager.Instance.PlaySE("UI_On");
        else
            Sound_Manager.Instance.PlaySE("UI_Off");
    }

    // Option UI 호출
    public void Spawn_Option_UI(bool set_Bool)
    {
        option.SetActive(set_Bool);

        // Sound

        Sound_Manager.Instance.StopAll_SFX();
        if (set_Bool)
            Sound_Manager.Instance.PlaySE("UI_On");
        else
            Sound_Manager.Instance.PlaySE("UI_Off");
    }

    // 다시하기 / 로비로 돌아가기
    public void Reset_Stage(bool restart)
    {
        ui_Anim.SetTrigger("Fade");

        if (restart)
            Invoke("Restart_Scene", 0.5f);
        else
            Invoke("Lobby_Scene", 0.5f);
    }

    // 게임종료
    public void Game_Exit()
    {
        Application.Quit();
    }

    #region Invoke Method
    void Restart_Scene()
    {
            SceneManager.LoadScene(cur_Scene.name);
    }
    void Lobby_Scene()
    {
            SceneManager.LoadScene("Lobby");
    }
    #endregion

    // Stage 대본 DB 적용
    private void Title_Text(string path, int i)
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //DataBase에서 title.json파일 찾아오기
        string jsonString = File.ReadAllText(path + "/title.json");
        JsonData itemData = JsonMapper.ToObject(jsonString);

        // Json Text 적용
        title_Level_Text.text = $"{itemData[i]["Title_Level"]}";
        title_Name_Text.text = $"{itemData[i]["Title_Name"]}";
        title_Detail_Text.text = $"{itemData[i]["Title_Detail"]}";

        // BGM Stage 변경
        Sound_Manager.Instance.StopAll_BGM();
        Sound_Manager.Instance.Play_SB("BGM_Stage");
    }

    // Audio Slider 설정
    public void BGM_Contorl()
    {
        mixer.SetFloat("BGM", Mathf.Log10(bgm_Slider.value) * 20);
    }
    public void SFX_Contorl()
    {
        mixer.SetFloat("SFX", Mathf.Log10(sfx_Slider.value) * 20);
    }

}
