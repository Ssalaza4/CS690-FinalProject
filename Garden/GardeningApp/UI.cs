namespace GardeningApp;

using Spectre.Console;

public class UI
{
    public void ShowMainMenu()
    {
        List<Plant> plants = DataManager.LoadPlants();

        var menuOptions = new List<string>
        {
            "View Plants",
            "Add Plant",
            "Remove Plant",
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
        }
    }

    private static void ViewPlants(List<Plant> plants)
    {
        if (plants.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No plants found.[/]");
            return;
        }

        foreach (var plant in plants)
        {
            AnsiConsole.MarkupLine(
                $"[blue]{plant.Id}[/] - {plant.Name}");
        }
    }

    private static void AddPlant(List<Plant> plants)
    {
        string name = AnsiConsole.Ask<string>("Enter plant [green]name[/]:");

        int nextId = 1;

        if (plants.Count > 0)
        {
            nextId = plants.Max(p => p.Id) + 1;
        }

        var plant = new Plant(nextId, name);

        plants.Add(plant);

        AnsiConsole.MarkupLine("[green]Plant added.[/]");
    }

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
                plants.RemoveAt(i);
                break;
            }
        }

        AnsiConsole.MarkupLine("[green]Plant removed.[/]");
    }
}