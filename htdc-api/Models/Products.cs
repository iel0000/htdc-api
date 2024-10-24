namespace htdc_api.Models;

public class Products: BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    public string Code { get; set; }
    public string Image { get; set; }
}