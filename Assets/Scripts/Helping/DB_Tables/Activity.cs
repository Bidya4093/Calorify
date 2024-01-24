using SQLite4Unity3d;

public class Activity
{

    [PrimaryKey, AutoIncrement]
    public int activity_id { get; set; }
    public string activity_name { get; set; }
    public int burns_calories { get; set; }
    public override string ToString()
    {
        return $"{activity_id}, {activity_name}, {burns_calories};";
    }
}
