using SQLite4Unity3d;

public class Daily_history
{

    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string date { get; set; }
    public float calories { get; set; }
    public float water_amount { get; set; }
    public float prots { get; set; }
    public float fats { get; set; }
    public float carbs { get; set; }

    public override string ToString()
    {
        return $"{id}, {date}, {calories}, {water_amount};";
    }
}