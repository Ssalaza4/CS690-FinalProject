namespace GardeningApp;

public class DataManager
{
    public static List<Plant> LoadPlants()
    {
        var plants = new List<Plant>();

        if (!File.Exists("plants.txt"))
            return plants;

        string[] lines = File.ReadAllLines("plants.txt");

        foreach (string line in lines)
        {
            string[] parts = line.Split(':');

            if (parts.Length == 2)
            {
                int id = int.Parse(parts[0]);
                string name = parts[1];

                var plant = new Plant(id, name);
                plants.Add(plant);
            }
        }

        return plants;
    }

    public static void SavePlants(List<Plant> plants)
    {
        var lines = new List<string>();

        foreach (var plant in plants)
        {
            string line = plant.Id + ":" + plant.Name;
            lines.Add(line);
        }

        File.WriteAllLines("plants.txt", lines);
    }
}