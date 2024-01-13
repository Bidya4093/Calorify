using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using Unity.VisualScripting;
using System;

public class DBWorking : MonoBehaviour
{
    static string dbName = "URI=file:Users.db";
    static public User user;
    void Start()
    {
        CreateDB();
        //User.SetAll("Max", "989m66@gmail.com", "lalalala", 0, 1, 82, 162);
        //RegisterUser();
        //LoginUser("bohdan@gmail.com", "qwerty123");
        //ShowAll();
        //ClearTable();
    }

    void CreateDB()
    {
        using(var connection = new SqliteConnection(dbName))
        {
            connection.Open(); 
            using(var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS User (ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Email TEXT, Password TEXT, Mass REAL, Height REAL, Goal INTEGER, ActivityLevel INTEGER);";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    static public void RegisterUser()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO User (Name, Email, Password, Mass, Height, Goal, ActivityLevel) VALUES (@Name, @Email, @Password, @Mass, @Height, @Goal, @ActivityLevel);";
                command.Parameters.AddWithValue("@Name", user.GetUsername());
                command.Parameters.AddWithValue("@Email", user.GetEmail());
                command.Parameters.AddWithValue("@Mass", user.GetWeight());
                command.Parameters.AddWithValue("@Height", user.GetHeight());
                command.Parameters.AddWithValue("@Goal", user.GetGoal());
                command.Parameters.AddWithValue("@ActivityLevel", user.GetActivity());
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
        // Task: Check if user already exists
    }

    static public void LoginUser(string email, string password)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM User WHERE Email = @Email AND Password = @Password;";
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //user.SetAll(SexType.Male, reader["Name"].ToString(), reader["Email"].ToString(), Convert.ToInt16(reader["Goal"]), Convert.ToInt16(reader["ActivityLevel"]), float.Parse(reader["Mass"].ToString()), float.Parse(reader["Height"].ToString()));
                    }
                }
            }
        }
        if (user.GetEmail().Length == 0)
        {
            throw new Exception("User not exist!");
        }
    }

    // Only for development
    static public void ShowAll()
    {
        using(var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using(var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM User;";
                using(IDataReader  reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log($"ID: {reader["ID"]}");
                        Debug.Log($"Name: {reader["Name"]}");
                        Debug.Log($"Email: {reader["Email"]}");
                        Debug.Log($"Password: {reader["Password"]}");
                        Debug.Log($"Mass: {reader["Mass"]}");
                        Debug.Log($"Height: {reader["Height"]}");
                        Debug.Log($"Goal: {reader["Goal"]}");
                        Debug.Log($"ActivityLevel: {reader["ActivityLevel"]}");
                    }
                }
            }
            connection.Close();

        }
    }


    static public void ClearTable()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DROP TABLE User;";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

}
