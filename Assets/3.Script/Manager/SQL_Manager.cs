using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System.IO;
using System;
using LitJson;

public class User_Info
{
    public string u_Name { get; private set; }
    public string u_Password { get; private set; }

    public User_Info(string name, string password)
    {
        u_Name = name;
        u_Password = password;
    }
}

public class SQL_Manager : MonoBehaviour
{

    [Header("DB Data")]
    [Header("=======================================")]
    public string DB_Path = string.Empty;
    public User_Info info;

    [Header("SQL Data")]
    [Header("=======================================")]
    public MySqlConnection sql_Connection;
    public MySqlDataReader sql_Reader;
    public static SQL_Manager Instance = null;

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
        
        //DB 경로
        DB_Path = Application.dataPath + "/DataBase";

        //aws의 db와 연결
        string serverinfo =  Server_Set(DB_Path);
        try
        {
            if (serverinfo.Equals(string.Empty))
            {
                //연결 실패
                Debug.Log("Sql Server is null! not connect");
                return;
            }

            sql_Connection = new MySqlConnection(serverinfo);
            sql_Connection.Open();
            Debug.Log("SQL Open Compelete!");
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    //AWS의 DB와 연결
    private string Server_Set(string path)
    {
        //해당 경로가 있는지 확인
        if (!File.Exists(path))
        {
            //없을 경우 경로 생성
            Directory.CreateDirectory(path);
        }

        //경로가 있다면 config.json파일을 읽어오기
        string jsonString = File.ReadAllText(path + "/config.json");

        JsonData itemData = JsonMapper.ToObject(jsonString);
        string serverInfo = 
            $"Server={itemData[0]["IP"]}; Database={itemData[0]["TableName"]}; Uid={itemData[0]["ID"]};" +
            $"Pwd={itemData[0]["PW"]}; Port={itemData[0]["PORT"]}; CharSet= utf8;";
        return serverInfo;
    }

    private bool Connect_Check(MySqlConnection connect)
    {
        //현재 mysql 상태가 아니라면
        if(connect.State != System.Data.ConnectionState.Open)
        {
            connect.Open();
            if(connect.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
        //오픈 상태
        return true;
    }

    public bool Login(string id, string Password)
    {
        //직접적 DB 조회
        try
        {
            if (!Connect_Check(sql_Connection))
            {
                //Connection을 체크
                return false;
            }

            //쿼리문으로 아이디와 비밀번호 데이터 확인
            string sql_command =
                string.Format(@"SELECT U_Name,U_Password FROM user_info WHERE U_Name = '{0}' AND U_Password = '{1}';", id, Password);
            MySqlCommand cmd = new MySqlCommand(sql_command, sql_Connection);
            sql_Reader = cmd.ExecuteReader();

            //Reader에서 읽은 데이터가 1개 이상 존재할 경우
            if (sql_Reader.HasRows)
            {
                while (sql_Reader.Read())
                {
                    //삼항연산자 계산

                    string name = (sql_Reader.IsDBNull(0)) ? string.Empty : (string)sql_Reader["U_Name"].ToString();
                    string pass = (sql_Reader.IsDBNull(1)) ? string.Empty : (string)sql_Reader["U_Password"].ToString();

                    if (!name.Equals(string.Empty) || !pass.Equals(string.Empty))
                    {
                        //로그인 성공
                        info = new User_Info(name, pass);

                        if (!sql_Reader.IsClosed) sql_Reader.Close();
                        return true;
                    }
                    else
                    {
                        //로그인 실패
                        break;
                    }
                }
            }
            if (!sql_Reader.IsClosed) sql_Reader.Close();
            return false;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    //회원가입
    public void Sign_Up(string id, string pw)
    {
        //쿼리문으로 DB의 데이터 테이블에 정보값 추가
        string cmd = string.Format(@"INSERT INTO user_info VALUES('{0}','{1}','{2}');", id, pw,0);
        using (MySqlCommand command = new MySqlCommand(cmd, sql_Connection))
        {
            command.ExecuteNonQuery();
        }
        info = new User_Info(id, pw);
    }
}
