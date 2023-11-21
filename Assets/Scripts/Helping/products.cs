using SQLite4Unity3d;

public class products
{

    [PrimaryKey, AutoIncrement]
    public int product_id { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string nutri_score { get; set; }
    public float calories { get; set; }
    public float prots { get; set; }
    public float fats { get; set; }
    public float carbs { get; set; }

    public override string ToString()
    {
        return $"{product_id}, {name}, {type}, {nutri_score}, {calories}, {prots}, {fats}, {carbs};";//string.Format("[products: product_id={0}, name={1},  type={2}, calories={3}, prots={4}, fats={5}, carbs={6}]", product_id, name, type, calories, prots, fats, carbs);
    }
}
