using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login_Manager : MonoBehaviour
{
    public InputField input_ID;
    public InputField input_Password;
    [SerializeField] private Text status_Text;
    [SerializeField] private GameObject[] info_Pages;

    public void Login_Event()
    {
        Sound_Manager.Instance.PlaySE("Select_Level");
        if (input_ID.text.Equals(string.Empty) || input_Password.text.Equals(string.Empty))
        {
            status_Text.text = "ID Ȥ�� Password�� �Է����ּ���.";
            return;
        }
        if (SQL_Manager.Instance.Login(input_ID.text, input_Password.text))
        {
            //�α��� ����
            User_Info info = SQL_Manager.Instance.info;
            Debug.Log(info.u_Name + " | " + info.u_Password);
            gameObject.SetActive(false);
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            //�α��� ����
            status_Text.text = "ID Ȥ�� Password�� �߸��Ǿ����ϴ�.";
            return;
        }
    }
    //ȸ�����Ե� �����
    public void Sign_Up()
    {
        Sound_Manager.Instance.PlaySE("Select_Level");

        if (input_ID.text.Equals(string.Empty) || input_Password.text.Equals(string.Empty))
        {
            status_Text.text = "ID Ȥ�� Password�� �Է����ּ���.";
            return;
        }
        else
        {
            //ȸ������ ����
            SQL_Manager.Instance.Sign_Up(input_ID.text, input_Password.text);
            User_Info info = SQL_Manager.Instance.info;
            status_Text.text = "- ȸ�������� ��ϵǾ����ϴ� -";
            Debug.Log(info.u_Name + " | " + info.u_Password);

  
            Sign_Page(false);
        }
    }

    public void Sign_Page(bool sign)
    {
        if (sign)
        {
            info_Pages[0].SetActive(false);
            info_Pages[1].SetActive(true);
            status_Text.text = "Sign Up";
        }
        else
        {
            info_Pages[1].SetActive(false);
            info_Pages[0].SetActive(true);

        }
        input_ID.text = null;
        input_Password.text = null;
    }
}
