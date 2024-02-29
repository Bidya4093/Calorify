using Firebase.Auth;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;
using System;
using static UnityEditor.Progress;

public class NotificationDBManager
{
    //public List<Notification> notificationsList;
    private List<Notification> records;
    private List<Notification> currentUserRecords;
    SQLiteConnection connection;

    public NotificationDBManager() 
    {
        var dataService = new DataService("notificationsdb.db");
        connection = dataService.GetConnection();
        records = new List<Notification>();
        ToList(connection.Table<Notification>());
        currentUserRecords = new List<Notification>();
        foreach (Notification record in records)
        {
            if (record.user_id == FirebaseAuth.DefaultInstance.CurrentUser.UserId) currentUserRecords.Add(record);
        }
    }

    public List<Notification> GetHistory()
    {
        return records;
    }

    public List<Notification> GetCurrentUserHistory()
    {
        return currentUserRecords;
    }

    public Notification InsertRecord(string title, string message, string type, bool is_new, string user_id)
    {
        DateTime date = DateTime.Now;
        Notification newRecord = new Notification
        {
            title = title,
            message = message,
            type = type,
            date = date.ToString("HH:mm     dd.MM.yyyy"),
            is_new = Convert.ToInt32(is_new),
            user_id = user_id
        };

        connection.Insert(newRecord);
        return newRecord;
    }

    public Notification InsertRecord(Notification newRecord)
    {
        connection.Insert(newRecord);
        return newRecord;
    }

    // Deletes the record with the given index
    public void DeleteRecordById(int id)
    {
        Notification recordToDelete = connection.Find<Notification>(id);
        if (recordToDelete != null)
        {
            connection.Delete(recordToDelete);
        }
        else
        {
            Debug.Log("Id not found!");
        }
    }

    public void UpdateStateNew(int id, bool isNew)
    {
        Notification updatedRecord = connection.Find<Notification>(id);
        if (updatedRecord != null)
        {
            updatedRecord.is_new = Convert.ToInt32(isNew);
            connection.Update(updatedRecord);
        } else
        {
            Debug.Log("Id not found!");
        }
    }

    // returns activity by its id. If not found - returns null
    public Notification GetById(int id)
    {
        return connection.Find<Notification>(id);
    }

    private void ToList(IEnumerable<Notification> activities)
    {
        foreach (var activity in activities)
        {
            records.Add(activity);
        }
    }
}