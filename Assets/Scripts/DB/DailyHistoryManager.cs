using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class DailyHistoryManager : MonoBehaviour
{
    SQLiteConnection connection;
    List<Daily_history> records;

    public DailyHistoryManager()
    {
        DataService dataService = new DataService("meals_history.db");
        connection = dataService.GetConnection();
        records = new List<Daily_history>();
        ToList(connection.Table<Daily_history>());
    }

    public List<Daily_history> GetHistory()
    {
        return records;
    }

    // Inserting new records. Reconnecting recommended
    public void InsertRecord(string date, float calories, float water_amount, float prots, float fats, float carbs)
    {
        Daily_history newRecord = new Daily_history
        {
            date = date,
            calories = calories,
            water_amount = water_amount,
            prots = prots,
            fats = fats,
            carbs = carbs
        };
        connection.Insert(newRecord);
    }

    private void ToList(IEnumerable<Daily_history> rows)
    {
        foreach (var record in rows)
        {
            records.Add(record);
        }
    }
}
