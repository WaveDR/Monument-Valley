using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login_Manager : MonoBehaviour
{
    [Header("Login UI")]
    [Header("=======================================")]
    public InputField input_ID;
    public InputField input_Password;
    [SerializeField] private Text status_Text;

    [Header("Login Page")]
    [Header("=======================================")]
    [SerializeField] private GameObject[] info_Pages;


    //로그인 버튼 
    public void Login_Event()
    {
        Sound_Manager.Instance.PlaySE("Select_Level");

        //ID or Password Empty
        if (input_ID.text.Equals(string.Empty) || input_Password.text.Equals(string.Empty))
        {
            status_Text.text = "ID 혹은 Password를 입력해주세요.";
            return;
        }

        //Login Sucess
        if (SQL_Manager.Instance.Login(input_ID.text, input_Password.text))
        {
            //로그인 성공
            User_Info info = SQL_Manager.Instance.info;
            Debug.Log(info.u_Name + " | " + info.u_Password);
            gameObject.SetActive(false);
            SceneManager.LoadScene("Lobby");
        }

        //Login Failed
        else
        {
            //로그인 실패
            status_Text.text = "ID 혹은 Password가 잘못되었습니다.";
            return;
        }
    }

    //회원가입 버튼
    public void Sign_Up()
    {
        Sound_Manager.Instance.PlaySE("Select_Level");

        //ID or Password Empty
        if (input_ID.text.Equals(string.Empty) || input_Password.text.Equals(string.Empty))
        {
            status_Text.text = "ID 혹은 Password를 입력해주세요.";
            return;
        }

        //SignUp Sucess
        else
        {
            SQL_Manager.Instance.Sign_Up(input_ID.text, input_Password.text);
            User_Info info = SQL_Manager.Instance.info;
            status_Text.text = "- 회원정보가 등록되었습니다 -";
            Debug.Log(info.u_Name + " | " + info.u_Password);
            Sign_Page(false);
        }
    }

    //회원가입 페이지 On / Off Method
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

        //InputField Reset
        input_ID.text = null;
        input_Password.text = null;
    }
}
