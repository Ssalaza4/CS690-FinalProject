using GardeningApp;

namespace GardeningApp.Tests;

public class DomainTests
{
    [Fact]
    public void CreatePlant_AssignsIdAndName()
    {
        Plant plant = new Plant(1, "Rose");

        Assert.Equal(1, plant.Id);
        Assert.Equal("Rose", plant.Name);
    }

    [Fact]
    public void CreatePlant_HasEmptySchedule()
    {
        Plant plant = new Plant(1, "Rose");

        Assert.NotNull(plant.Schedule);
        Assert.Equal("", plant.Schedule.WateringDays);
        Assert.Equal("", plant.Schedule.PruningDays);
    }

    [Fact]
    public void CreateSchedule_AssignsDays()
    {
        CareSchedule schedule = new CareSchedule();

        schedule.WateringDays = "Mon,Wed";
        schedule.PruningDays = "Sun";

        Assert.Equal("Mon,Wed", schedule.WateringDays);
        Assert.Equal("Sun", schedule.PruningDays);
    }
    [Fact]
    public void CreateCareActivity_AssignsValues()
    {
        DateTime date = new DateTime(2026, 4, 30);

        CareActivity activity = new CareActivity(1, "Watered", date);

        Assert.Equal(1, activity.PlantId);
        Assert.Equal("Watered", activity.ActivityType);
        Assert.Equal(date, activity.ActivityDate);
    }
}