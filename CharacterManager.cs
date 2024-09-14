namespace CharacterConsole;

public class CharacterManager
{
    private readonly IInput _input;
    private readonly IOutput _output;
    private readonly string _filePath = "input.csv";
    public List<Character> Characters { get; set; }

    private string[] _lines;

    public CharacterManager(IInput input, IOutput output)
    {
        _input = input;
        _output = output;
    }

    public void Run()
    {
        _output.WriteLine("Welcome to Character Management");

        _lines = File.ReadAllLines(_filePath);

        while (true)
        {
            _output.WriteLine("Menu:");
            _output.WriteLine("1. Display Characters");
            _output.WriteLine("2. Find Character");
            _output.WriteLine("2. Add Character");
            _output.WriteLine("4. Level Up Character");
            _output.WriteLine("5. Exit");
            _output.Write("Enter your choice: ");
            var choice = _input.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayCharacters();
                    break;
                case "2":
                    FindCharacters();
                    break;
                case "3":
                    AddCharacter();
                    break;
                case "4":
                    LevelUpCharacter();
                    break;
                case "5":
                    return;
                default:
                    _output.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public void DisplayCharacters()
    {
        // TODO: Implement displaying characters from the CSV file
        
        for (int i = 1; i < _lines.Length; i++)
        {
            string line = _lines[i];

            string name;
            int commaIndex = line.IndexOf(",");
            
            if (line.StartsWith("\""))
            {
                var firstQuote = line.IndexOf("\"");
                line = line.Substring(firstQuote + 1);
                var lastQuote = line.IndexOf("\"");
                commaIndex = line.IndexOf(",", lastQuote);
                name = line.Substring(0, commaIndex-1);
            }
            else
            {
                name = line.Substring(0, commaIndex);
            }
            
            string[] fields = line.Split(',');
            string characterClass = fields[fields.Length - 4];
            
            int level = Convert.ToInt32(fields[fields.Length - 3]); 
            
            int hitPoints = Convert.ToInt32(fields[fields.Length - 2]); 
            
            string[] equipment = fields[fields.Length - 1].Split("|");
            
            Console.WriteLine($"Name: {name}, Class: {characterClass}, Level: {level}, HP: {hitPoints}, Equipment: {string.Join(", ", equipment)}");
        }
    }

    public void FindCharacters()
    {
        Characters = new List<Character>();
        for (int i = 1; i < _lines.Length; i++)
        {
            string line = _lines[i];
            string name = line.Substring(1, line.IndexOf(",") - 1);
            Characters.Add(new Character() { Name = name });
        }

        Console.Write("Enter the name of character to find: ");
        string nameToFind = Console.ReadLine();
        
        var foundCharacter = Characters.Where(c => c.Name == nameToFind).FirstOrDefault();
        
        Console.WriteLine($"Name: {foundCharacter?.Name}");
    }
    
    public void AddCharacter()
    {
        Console.WriteLine("What is the characters name:");
        string name = Console.ReadLine();
        Console.WriteLine("What is the characters class:");
        string characterClass = Console.ReadLine();
        Console.WriteLine("What is the characters level:");
        int level = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("What is the characters hit points:");
        int hitPoints = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Equipment 1:");
        string equipment1 = Console.ReadLine();
        Console.WriteLine("Equipment 2:");
        string equipment2 = Console.ReadLine();
        Console.WriteLine("Equipment 3:");
        string equipment3 = Console.ReadLine();

        string character = $"{name},{characterClass},{level},{hitPoints},{equipment1}|{equipment2}|{equipment3}";

        Array.Resize(ref _lines, _lines.Length + 1);
        _lines[_lines.Length-1] = character;
    }

    public void LevelUpCharacter()
    {
        // TODO: Implement leveling up a character and updating the CSV file
        Console.Write("Enter the name of the character to level up: ");
        string nameToLevelUp = Console.ReadLine();
        
        for (int i = 1; i < _lines.Length; i++)
        {
            string line = _lines[i];
            
            if (line.Contains(nameToLevelUp))
            {
                string[] fields = line.Split(',');
                int level = Convert.ToInt32(fields[fields.Length - 3]);
                string name = line.Substring(1, line.IndexOf(",")-1);
                
                level++;
                Console.WriteLine($"Character {name} leveled up to level {level}!");
                fields[fields.Length - 3] = Convert.ToString(level);
                
                string character = String.Join(",", fields);
                _lines[i] = character;
                break;
            }
        }
    }
}
