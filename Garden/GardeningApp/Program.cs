using Spectre.Console;

List<Dictionary<string, string>> plants = LoadPlants();

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
        SavePlants(plants);
    }
    else if (choice == "Remove Plant")
    {
        RemovePlant(plants);
        SavePlants(plants);
    }
    else if (choice == "Exit")
    {
        SavePlants(plants);
        break;
    }
}

// ==========================
// FUNCTIONS
// ==========================

static List<Dictionary<string, string>> LoadPlants()
{
    var plants = new List<Dictionary<string, string>>();

    if (!File.Exists("plants.txt"))
        return plants;

    string[] lines = File.ReadAllLines("plants.txt");

    foreach (string line in lines)
    {
        string[] parts = line.Split(':');

        if (parts.Length == 2)
        {
            var plant = new Dictionary<string, string>();
            plant["Id"] = parts[0];
            plant["Name"] = parts[1];

            plants.Add(plant);
        }
    }

    return plants;
}

static void SavePlants(List<Dictionary<string, string>> plants)
{
    var lines = new List<string>();

    foreach (var plant in plants)
    {
        string line = plant["Id"] + ":" + plant["Name"];
        lines.Add(line);
    }

    File.WriteAllLines("plants.txt", lines);
}

static void ViewPlants(List<Dictionary<string, string>> plants)
{
    if (plants.Count == 0)
    {
        AnsiConsole.MarkupLine("[red]No plants found.[/]");
        return;
    }

    foreach (var plant in plants)
    {
        AnsiConsole.MarkupLine(
            $"[blue]{plant["Id"]}[/] - {plant["Name"]}");
    }
}

static void AddPlant(List<Dictionary<string, string>> plants)
{
    string name = AnsiConsole.Ask<string>("Enter plant [green]name[/]:");

    int nextId = 1;

    if (plants.Count > 0)
    {
        nextId = plants.Max(p => int.Parse(p["Id"])) + 1;
    }

    var plant = new Dictionary<string, string>();
    plant["Id"] = nextId.ToString();
    plant["Name"] = name;

    plants.Add(plant);

    AnsiConsole.MarkupLine("[green]Plant added.[/]");
}

static void RemovePlant(List<Dictionary<string, string>> plants)
{
    if (plants.Count == 0)
    {
        AnsiConsole.MarkupLine("[red]No plants to remove.[/]");
        return;
    }

    var choices = new List<string>();
    foreach (var plant in plants)
    {
        choices.Add(plant["Id"] + " - " + plant["Name"]);
    }

    var selected = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select a plant to [red]remove[/]:")
            .AddChoices(choices));

    string selectedId = selected.Split('-')[0].Trim();

    for (int i = 0; i < plants.Count; i++)
    {
        if (plants[i]["Id"] == selectedId)
        {
            plants.RemoveAt(i);
            break;
        }
    }

    AnsiConsole.MarkupLine("[green]Plant removed.[/]");
}