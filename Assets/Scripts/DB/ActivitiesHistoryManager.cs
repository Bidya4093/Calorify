using Firebase.Auth;
using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;

public class ActivitiesHistoryManager
{
    SQLiteConnection connection;
    public ActivitiesHistoryManager()
    {
        DataService dataService = new DataService("activities_history.db");
        connection = dataService.GetConnection();
    }

    public List<ActivitiesHistory> GetRecordsForToday()
    {
        string dateTime = DateTime.Now.ToString("yyyy.MM.dd");
        string user_id = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        return connection.Table<ActivitiesHistory>().Where(tag => tag.user_id == user_id && tag.date == dateTime).ToList();
    }

    private List<ActivitiesHistory> GetAllRecords()
    {
        string user_id = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        return connection.Table<ActivitiesHistory>().Where(tag => tag.user_id == user_id).ToList();
    }

    public void InsertRecord(ActivitiesHistory record)
    {
        connection.Insert(record);
    }

    public void InsertRecord(int activity_id, int time_spent, float calories_burnt)
    {
        ActivitiesHistory newRecord = new ActivitiesHistory
        {
            activity_id = activity_id,
            time_spent = time_spent,
            calories_burnt = calories_burnt,
            date = DateTime.Now.ToString("yyyy.MM.dd"),
            time = DateTime.Now.ToString("HH:mm"),
            user_id = FirebaseAuth.DefaultInstance.CurrentUser.UserId
        };
        connection.Insert(newRecord);
    }

    public void DeleteRecord(int id)
    {
        ActivitiesHistory recordToDelete = connection.Table<ActivitiesHistory>().Where(tag => tag.id == id).FirstOrDefault();
        if (recordToDelete != null)
        {
            connection.Delete(recordToDelete);
        }
        else
        {
            throw new Exception("Id not found!");
        }
    }

    public void UpdateTimeSpent(int id, int time_spent)
    {
        ActivitiesHistory updatedRecord = connection.Table<ActivitiesHistory>().Where(tag => tag.id == id).FirstOrDefault();
        if (updatedRecord != null)
        {
            updatedRecord.time_spent = time_spent;
            connection.Update(updatedRecord);
        }
        else
        {
            throw new Exception("Id not found!");
        }
    }

}
