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
    public float u_Time { get; private set; }

    public User_Info(string name, string password)
    {
        u_Name = name;
        u_Password = password;
    }
    public void Set_Name(string name)
    {
        u_Name = name;
    }
}


public class SQL_Manager : MonoBehaviour
{
    public MySqlConnection sql_Connection;
    public MySqlDataReader sql_Reader;
    public User_Info info;
    public string DB_Path = string.Empty;

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
        DB_Path = Application.dataPath + "/DataBase";
        string serverinfo =  Server_Set(DB_Path);

        try
        {
            if (serverinfo.Equals(string.Empty))
            {
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
    private string Server_Set(string path)
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
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


    public void Sign_Up(string id, string pw)
    {
        string cmd = string.Format(@"INSERT INTO user_info VALUES('{0}','{1}','{2}');", id, pw,0);
        using (MySqlCommand command = new MySqlCommand(cmd, sql_Connection))
        {
            command.ExecuteNonQuery();
        }
        info = new User_Info(id, pw);
    }
}
