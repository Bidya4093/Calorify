using SQLite4Unity3d;

public class Todays_history
{

    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public int product_id { get; set; }
    public float mass { get; set; }
    public float water_amount { get; set; }
    public string date { get; set; }

    public override string ToString()
    {
        return $"{id}, {product_id}, {mass};";
    }
}