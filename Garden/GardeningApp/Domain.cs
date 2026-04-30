namespace GardeningApp;

public class Plant
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Plant() { }

    public Plant(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}