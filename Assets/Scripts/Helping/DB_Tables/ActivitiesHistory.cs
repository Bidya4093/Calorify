using SQLite4Unity3d;

public class ActivitiesHistory
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public int activity_id { get; set; }
    public string user_id { get; set; }
    public int time_spent { get; set; }
    public float calories_burnt { get; set; }
    public string date { get; set; }
    public string time { get; set; }

    public override string ToString()
    {
        return $"{id}, {activity_id}, {user_id}, {time_spent}, {calories_burnt}, {date}, {time};";
    }

}
