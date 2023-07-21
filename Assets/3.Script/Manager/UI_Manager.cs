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

        //���� Scene �̸� ��������
        cur_Scene = SceneManager.GetActiveScene();

        //Player �г����� Sql���� ��������
        player_Name.text = SQL_Manager.Instance.info.u_Name;

        //DB ��� ����
        title_Path = Application.dataPath + "/DataBase";

        //ETC Reset
        setting_Base.SetActive(false);
        bgm_Slider.value = 0.5f;
        sfx_Slider.value = 0.5f;
    }
    private void Start()
    {
        //���� Scene�� Lobby�� Return;
        if (cur_Scene.name.Equals("Lobby"))
        {
            title_Obejct.SetActive(false);
            return;
        }

        //Json DB�� Ÿ��Ʋ �뺻 ����
        else if (cur_Scene.name.Equals("Stage01"))
        {
            Title_Text(title_Path, 0); // DB���, Stage ����
        }
        else
        {
            Title_Text(title_Path, 1); // DB���, Stage ����
        }
    }

    private void Update()
    {
        //Stage�� ��� Ŭ������ ���� �Ѿ��
        if (Input.GetMouseButtonDown(0))
        {
            ui_Anim.SetBool("GameStart", true);
            title_Obejct.SetActive(false);
        }
    }


    //==================================================== Basic Method / CallBack Method =======================================================


    // ��� UI ȣ��
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

    // ������ UI ȣ��
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

    // Option UI ȣ��
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

    // �ٽ��ϱ� / �κ�� ���ư���
    public void Reset_Stage(bool restart)
    {
        ui_Anim.SetTrigger("Fade");

        if (restart)
            Invoke("Restart_Scene", 0.5f);
        else
            Invoke("Lobby_Scene", 0.5f);
    }

    // ��������
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

    // Stage �뺻 DB ����
    private void Title_Text(string path, int i)
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //DataBase���� title.json���� ã�ƿ���
        string jsonString = File.ReadAllText(path + "/title.json");
        JsonData itemData = JsonMapper.ToObject(jsonString);

        // Json Text ����
        title_Level_Text.text = $"{itemData[i]["Title_Level"]}";
        title_Name_Text.text = $"{itemData[i]["Title_Name"]}";
        title_Detail_Text.text = $"{itemData[i]["Title_Detail"]}";

        // BGM Stage ����
        Sound_Manager.Instance.StopAll_BGM();
        Sound_Manager.Instance.Play_SB("BGM_Stage");
    }

    // Audio Slider ����
    public void BGM_Contorl()
    {
        mixer.SetFloat("BGM", Mathf.Log10(bgm_Slider.value) * 20);
    }
    public void SFX_Contorl()
    {
        mixer.SetFloat("SFX", Mathf.Log10(sfx_Slider.value) * 20);
    }

}
