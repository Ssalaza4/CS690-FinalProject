namespace GardeningApp;

using Spectre.Console;


// will be used in our program main to show ui and then perform all the necessary functions to allow the user to interact with the user
public class UI
{
    // creating a string list and then filling it with weekday options to be used to fill our careschedule 
    private static readonly string[] WeekDays = new[]
    {
        "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"
    };

    // will be used in our main to show our main menu and then call the appropriate method depending on user input. first uses our load plants method to create our plants list 
    public void ShowMainMenu()
    {
        List<Plant> plants = DataManager.LoadPlants();

        var menuOptions = new List<string>
        {
            "View Plants",
            "Add Plant",
            "Remove Plant",
            "Log Care Activity",
            "View Care History",
            "View Reminders",
            "Exit"
        };

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an [green]option[/]:")
                    .AddChoices(menuOptions));

            if (choice == "View Plants")
            {
                ViewPlants(plants);
            }
            else if (choice == "Add Plant")
            {
                AddPlant(plants);
                DataManager.SavePlants(plants);
            }
            else if (choice == "Remove Plant")
            {
                RemovePlant(plants);
                DataManager.SavePlants(plants);
            }
            else if (choice == "Exit")
            {
                DataManager.SavePlants(plants);
                break;
            }
            else if (choice == "Log Care Activity")
            {
                LogCareActivity(plants);
            }
            else if (choice == "View Care History")
            {
                ViewCareHistory(plants);
            }
            else if (choice == "View Reminders")
            {
                ViewReminders(plants);
            }
        }
    }

//**************
//Plant and inventory related methods
//***************

// this method takes in a list of our plants objects and either returns a no plants found if count of items is zero or iterates through the list and presents their Id, Plant Name, careschedule string object for each of watering and pruning. it also calls methods from the plant object that are methods of another class in the careschedule 'schedule' object for watering and pruning schedules
    private static void ViewPlants(List<Plant> plants)
    {
        if (plants.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No plants found.[/]");
            return;
        }

        foreach (var plant in plants)
        {
            AnsiConsole.MarkupLine($"[blue]{plant.Id}[/] - {plant.Name}");
            AnsiConsole.MarkupLine($"  Watering Days: [green]{plant.Schedule.WateringDays}[/]");
            AnsiConsole.MarkupLine($"  Pruning Days: [green]{plant.Schedule.PruningDays}[/]");
            AnsiConsole.WriteLine();
        }
    }

    // this method takes in our plants list then it asks for a string name (returns error if empty or space is input)
    private static void AddPlant(List<Plant> plants)
    {
        string name = AnsiConsole.Ask<string>("Enter plant [green]name[/]:");

        if (string.IsNullOrWhiteSpace(name))
        {
            AnsiConsole.MarkupLine("[red]Invalid name.[/]");
            return;
        }
        // uses our previously created weekday list to provide options and ask for user input to return weekdays that we will water our plants then we join our selection into a string of all days we will water
        var wateringSelection = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Select [green]watering days[/]:")
                .InstructionsText("[grey](Press <space> to toggle a day, <enter> to accept)[/]")
                .AddChoices(WeekDays));

        var pruningSelection = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Select [green]pruning days[/] (optional):")
                .InstructionsText("[grey](Press <space> to toggle a day, <enter> to accept)[/]")
                .AddChoices(WeekDays));

        string wateringDays = string.Join(",", wateringSelection);
        string pruningDays = string.Join(",", pruningSelection);

        // takes an int variable nextid assigns a 1 then iterates through all existing plant ids until it reacches adding a single number to the last plant id to assign the nextid - we use a 1 because the first entry will always be id number 1
        int nextId = 1;

        foreach (var plant in plants)
        {
            if (plant.Id >= nextId)
            {
                nextId = plant.Id + 1;
            }
        }

        Plant newPlant = new Plant(nextId, name);

        newPlant.Schedule.WateringDays = wateringDays;
        newPlant.Schedule.PruningDays = pruningDays;

        plants.Add(newPlant);

        AnsiConsole.MarkupLine("[green]Plant added.[/]");
    }

    // will take our list of plants return a comment if no plants exist in our list otherwise create a new string iterate through our list of plants to add them in our specified format use that list as our choices for user input to select which to delete then breakdown that into ID and use ID to remove our from our plants list 
    private static void RemovePlant(List<Plant> plants)
    {
        if (plants.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No plants to remove.[/]");
            return;
        }

        var choices = new List<string>();

        foreach (var plant in plants)
        {
            choices.Add(plant.Id + " - " + plant.Name);
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a plant to [red]remove[/]:")
                .AddChoices(choices));

        string selectedId = selected.Split('-')[0].Trim();

        for (int i = 0; i < plants.Count; i++)
        {
            if (plants[i].Id.ToString() == selectedId)
            {
                DataManager.RemoveCareHistoryForPlant(plants[i].Id);
                plants.RemoveAt(i);
                break;
            }
        }

        AnsiConsole.MarkupLine("[green]Plant removed.[/]");
    }

    // takes in our plants list and if plants count is 0 we return after message, otherwise we create a new choices list add each plant in our now standard format  and use them as option choices to select which plant will record a care activity
    private static void LogCareActivity(List<Plant> plants)
    {
        if (plants.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No plants available.[/]");
            return;
        }

        var choices = new List<string>();

        foreach (var plant in plants)
        {
            choices.Add(plant.Id + " - " + plant.Name);
        }

        var selectedPlant = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a plant:")
                .AddChoices(choices));

        int plantId = int.Parse(selectedPlant.Split('-')[0].Trim());

        // request the user select if we have watered or pruned our plants and then creates a new CareActivity class object with all the necessary paramters, creates our list of existing careactivities and then adds our new activity into the list followed by saving it 
        var activityType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select care activity:")
                .AddChoices("Watered", "Pruned"));

        CareActivity activity = new CareActivity(plantId, activityType, DateTime.Today);

        List<CareActivity> history = DataManager.LoadCareHistory();
        history.Add(activity);
        DataManager.SaveCareHistory(history);

        AnsiConsole.MarkupLine("[green]Care activity logged.[/]");
    }
    //used to display our existing care activity history - we have two seperate text files at work and we use our id as a means to join them 
    private static void ViewCareHistory(List<Plant> plants)
    {
        List<CareActivity> history = DataManager.LoadCareHistory();

        if (history.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No care history found.[/]");
            return;
        }

        foreach (var activity in history)
        {
            string plantName = "";

            foreach (var plant in plants)
            {
                if (plant.Id == activity.PlantId)
                {
                    plantName = plant.Name;
                    break;
                }
            }

            AnsiConsole.MarkupLine(
                $"[blue]{plantName}[/] - {activity.ActivityType} on {activity.ActivityDate:yyyy-MM-dd}");
        }
    }

    //allows a list of plants that need to be watered or pruned
    private static void ViewReminders(List<Plant> plants)
    {
        string today = DateTime.Today.ToString("ddd");

        var plantsToWater = new List<Plant>();
        var plantsToPrune = new List<Plant>();

        foreach (var plant in plants)
        {
            if (plant.Schedule.WateringDays.Contains(today))
            {
                plantsToWater.Add(plant);
            }

            if (plant.Schedule.PruningDays.Contains(today))
            {
                plantsToPrune.Add(plant);
            }
        }

        if (plantsToWater.Count == 0 && plantsToPrune.Count == 0)
        {
            AnsiConsole.MarkupLine("[green]No care reminders for today.[/]");
            return;
        }

        if (plantsToWater.Count > 0)
        {
            AnsiConsole.MarkupLine("[yellow]Plants needing water today:[/]");
            foreach (var plant in plantsToWater)
            {
                AnsiConsole.MarkupLine($"[blue]{plant.Id}-{plant.Name}[/]");
            }
        }

        if (plantsToPrune.Count > 0)
        {
            AnsiConsole.MarkupLine("[yellow]Plants needing pruning today:[/]");
            foreach (var plant in plantsToPrune)
            {
                AnsiConsole.MarkupLine($"[blue]{plant.Id}-{plant.Name}[/]");
            }
        }
    }
}