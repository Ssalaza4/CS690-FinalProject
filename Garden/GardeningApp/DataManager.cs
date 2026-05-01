namespace GardeningApp;

public class DataManager
{
    //will be used to either return an empty new plants list or iterate through our existing file to create a plant object for each line and return a list of our plants
    public static List<Plant> LoadPlants()
    {
        var plants = new List<Plant>();

        if (!File.Exists("plants.txt"))
            return plants;

        string[] lines = File.ReadAllLines("plants.txt");

        foreach (string line in lines)
        {
            string[] parts = line.Split(':');

            if (parts.Length == 4)
            {
                int id = int.Parse(parts[0]);
                string name = parts[1];
                string wateringDays = parts[2];
                string pruningDays = parts[3];

                Plant plant = new Plant(id, name);
                plant.Schedule.WateringDays = wateringDays;
                plant.Schedule.PruningDays = pruningDays;

                plants.Add(plant);
            }
        }

        return plants;
    }

    // will be used to save our current plants list into our plants.txt
    public static void SavePlants(List<Plant> plants)
    {
        var lines = new List<string>();

        foreach (var plant in plants)
        {
            string line =
                plant.Id + ":" +
                plant.Name + ":" +
                plant.Schedule.WateringDays + ":" +
                plant.Schedule.PruningDays;

            lines.Add(line);
        }

        File.WriteAllLines("plants.txt", lines);
    }

    //will be used to return either an empty history list or iterate through our care history txt file to create a careactivity object for each line and then adding to our list 
    public static List<CareActivity> LoadCareHistory()
    {
        var history = new List<CareActivity>();

        if (!File.Exists("care-history.txt"))
            return history;

        string[] lines = File.ReadAllLines("care-history.txt");

        foreach (string line in lines)
        {
            string[] parts = line.Split(':');

            if (parts.Length == 3)
            {
                int plantId = int.Parse(parts[0]);
                string activityType = parts[1];
                DateTime activityDate = DateTime.Parse(parts[2]);

                CareActivity activity = new CareActivity(plantId, activityType, activityDate);
                history.Add(activity);
            }
        }

        return history;
    }

    // will be used to save our care activity history
    public static void SaveCareHistory(List<CareActivity> history)
    {
        var lines = new List<string>();

        foreach (var activity in history)
        {
            string line =
                activity.PlantId + ":" +
                activity.ActivityType + ":" +
                activity.ActivityDate.ToString("yyyy-MM-dd");

            lines.Add(line);
        }

        File.WriteAllLines("care-history.txt", lines);
    }

    // used to remove care activity history if the corresponding plant was deleted 
    public static void RemoveCareHistoryForPlant(int plantId)
    {
        List<CareActivity> history = LoadCareHistory();

        for (int i = history.Count - 1; i >= 0; i--)
        {
            if (history[i].PlantId == plantId)
            {
                history.RemoveAt(i);
            }
        }

        SaveCareHistory(history);
    }
    
}