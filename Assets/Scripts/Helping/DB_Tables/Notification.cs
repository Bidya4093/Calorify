using SQLite4Unity3d;

public class Notification
{

    [PrimaryKey, AutoIncrement]
    public int notification_id { get; set; }
    public string user_id { get; set; }
    public string title { get; set; }
    public string message { get; set; }
    public string type { get; set; }
    public string date { get; set; }
    public int is_new { get; set; }
    public int viewed_as_push_message { get; set; }

    public override string ToString()
    {
        return $"id:{notification_id}, user_id:{user_id}, title:{title}, message:{message}, type:{type} date:{date} new:{is_new}; viewed_as_push_message: {viewed_as_push_message}";
    }
}