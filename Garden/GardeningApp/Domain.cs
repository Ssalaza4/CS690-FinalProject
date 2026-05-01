namespace GardeningApp;

// plant objects will serve as the basis of our project they will also contain careschedule which will contain schedules for watering and pruning, and then also care activity which will helps us record datetime for the activities we performed
public class Plant
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public CareSchedule Schedule { get; set; } = new CareSchedule();
    public List<CareActivity> CareHistory { get; set; } = new List<CareActivity>();

    public Plant() { }

    public Plant(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}

public class CareSchedule
{
    // we will use string type days to record because i do not have experience with date/time functions (we will only use date/time for recording a care activity and a simple reminder if feasible)
    public string WateringDays { get; set; } = "";
    public string PruningDays { get; set; } = "";
}

public class CareActivity
{
    // we want to link this back to a specific plant
    public int PlantId { get; set; }

    // example values: "Watered", "Pruned" - realistically only those will be included in our data as we do not want to interact with other types so much
    public string ActivityType { get; set; } = "";

    public DateTime ActivityDate { get; set; }

    public CareActivity() { }

    public CareActivity(int plantId, string activityType, DateTime activityDate)
    {
        this.PlantId = plantId;
        this.ActivityType = activityType;
        this.ActivityDate = activityDate;
    }
}