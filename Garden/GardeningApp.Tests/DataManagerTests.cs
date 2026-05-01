using GardeningApp;

namespace GardeningApp.Tests;

public class DataManagerTests
{
    [Fact]
    public void SaveLoad_ReturnTheSamePlant()
    {
        List<Plant> plants = new List<Plant>();

        Plant plant = new Plant(1, "Rose");
        plant.Schedule.WateringDays = "Mon,Wed";
        plant.Schedule.PruningDays = "Sun";

        plants.Add(plant);

        DataManager.SavePlants(plants);

        List<Plant> loadedPlants = DataManager.LoadPlants();

        Assert.Equal(1, loadedPlants.Count);
        Assert.Equal(1, loadedPlants[0].Id);
        Assert.Equal("Rose", loadedPlants[0].Name);
        Assert.Equal("Mon,Wed", loadedPlants[0].Schedule.WateringDays);
        Assert.Equal("Sun", loadedPlants[0].Schedule.PruningDays);
    }

    [Fact]
    public void SaveLoad_ReturnTheSameHistory()
    {
        List<CareActivity> history = new List<CareActivity>();

        DateTime date = new DateTime(2026, 4, 30);
        CareActivity activity = new CareActivity(1, "Watered", date);

        history.Add(activity);

        DataManager.SaveCareHistory(history);

        List<CareActivity> loadedHistory = DataManager.LoadCareHistory();

        Assert.Equal(1, loadedHistory.Count);
        Assert.Equal(1, loadedHistory[0].PlantId);
        Assert.Equal("Watered", loadedHistory[0].ActivityType);
        Assert.Equal(date, loadedHistory[0].ActivityDate);
    }
}