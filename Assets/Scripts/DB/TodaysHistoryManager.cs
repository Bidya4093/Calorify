using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TodaysHistoryManager : MonoBehaviour
{
    private List<Todays_history> records;
    SQLiteConnection connection;

    // Initialization where we create a new connection
    public TodaysHistoryManager()
    {
        DataService dataService = new DataService("meals_history.db");
        connection = dataService.GetConnection();
        records = new List<Todays_history>();
        ToList(connection.Table<Todays_history>());
    }

    public List<Todays_history> GetHistory()
    {
        return records;
    }

    // Inserting new records. Reconnecting recommended
    // When you insert water, specify that product_id = 22, mass = 0, water_amount - in liters
    // Otherwise, only set product_id and mass
    public void InsertRecord(int product_id, float mass, float water_amount = 0)
    {
        DateTime date = DateTime.Today;
        Todays_history newRecord = new Todays_history
        {
            product_id = product_id,
            mass = mass,
            water_amount = water_amount,
            date = date.ToString("yyyy-MM-dd")
        };
        connection.Insert(newRecord);
    }

    // Changes mass for the record with the given index
    public void UpdateMass(int id, float newMass)
    {
       Todays_history updatedRecord = connection.Table<Todays_history>().Where(tag => tag.id == id).FirstOrDefault();
       if (updatedRecord != null) {
            updatedRecord.mass = newMass;
            connection.Update(updatedRecord);
       } else
       {
            Debug.Log("Id not found!");
       }
    }

    // Deletes the record with the given index
    public void DeleteRecord(int id, float newMass)
    {
        Todays_history recordToDelete = connection.Table<Todays_history>().Where(tag => tag.id == id).FirstOrDefault();
        if (recordToDelete != null)
        {
            connection.Delete(recordToDelete);
        }
        else
        {
            Debug.Log("Id not found!");
        }
    }

    // Removes all data from the table
    public void ClearTable()
    {
        connection.DeleteAll<Todays_history>();
    }

    private void ToList(IEnumerable<Todays_history> rows)
    {
        foreach (var record in rows)
        {
            records.Add(record);
        }
    }
}
