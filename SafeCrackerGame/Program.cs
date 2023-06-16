using System;
using System.Collections.Generic;
using System.Linq;

class Multipliers
{
    private List<int> _multipliers;

    public Multipliers()
    {
        _multipliers = new List<int> { 15, 16, 17, 18, 19, 20 };
    }

    public List<int> LoadThreeRandomMultipliers()
    {
        List<int> shuffledMultipliers = _multipliers.OrderBy(_ => Guid.NewGuid()).ToList();
        return shuffledMultipliers.GetRange(0, 3);
    }
}

class Box
{
    private int _index;
    private int _multiplier;
    private bool _isOpen;

    public Box(int index, int multiplier)
    {
        _index = index;
        _multiplier = multiplier;
    }

    public void OpenBox()
    {
        _isOpen = true;
    }

    public bool GetIsOpen()
    {
        return _isOpen;
    }

    public int GetMultiplier()
    {
        return _multiplier;
    }

    public int GetIndex()
    {
        return _index;
    }
}

class Grid
{
    private List<Box> _boxes;
    private List<int> _multipliers;
    private Dictionary<int, int> _multipliersCount;
    private int _gridLength = 9;
    
    public Grid()
    {
        _multipliers = new Multipliers().LoadThreeRandomMultipliers();
        _multipliersCount = _multipliers.ToDictionary(multiplier => multiplier, _ => 0);
        _boxes = GenerateGrid();
    }
    
    private List<Box> GenerateGrid()
    {
        List<Box> grid = new List<Box>();
        int[] multipliersCount = { 0, 0, 0 };

        Random random = new Random();

        while (grid.Count < _gridLength)
        {
            int randomMultiplier = _multipliers[random.Next(_multipliers.Count)];

            if (multipliersCount[_multipliers.IndexOf(randomMultiplier)] < 3)
            {
                grid.Add(new Box(grid.Count + 1, randomMultiplier));
                multipliersCount[_multipliers.IndexOf(randomMultiplier)]++;
            }
        }

        return grid;
    }
    
    public void PrintGrid()
    {
        Console.WriteLine();
        Console.WriteLine("-------------------");
        string grid = "";
        for (int i = 0; i < _gridLength; i++)
        {
            Box box = _boxes[i];
            string boxContent = box.GetIsOpen() ? $"x{box.GetMultiplier()}" : box.GetIndex().ToString();
            if (boxContent.StartsWith('x'))
            {
                grid += $"| {boxContent} ";
            }
            else
            {
                grid += $"|  {boxContent}  ";
            }
            if ((i + 1) % 3 == 0)
            {
                grid += "|\n";
                grid += "-------------------\n";
            }
        }
        Console.WriteLine(grid);
    }

    public int GetRandomBoxToSpin()
    {
        bool continueVar = true;
        while (continueVar)
        {
            int randomNumber = new Random().Next(1, _gridLength);
            if (!_boxes[randomNumber].GetIsOpen())
            {
                continueVar = false;
                _multipliersCount[_boxes[randomNumber].GetMultiplier()]++;
                return randomNumber;
            }
        }

        return -1;
    }
    
    public int CheckWin()
    {
        foreach (var multiplier in _multipliers)
        {
            if (_multipliersCount[multiplier] == 3)
            {
                return multiplier;
            }
        }

        return -1;
    }

    public List<Box> GetBoxes()
    {
        return _boxes;
    }
}


class SafeCrackerGame
{
    private Grid _grid;
    private int _betAmount;

    public SafeCrackerGame()
    {
        _grid = new Grid();
    }

    private void SpinBox()
    {
        Console.WriteLine("Press SPACE to spin!");
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        while (keyInfo.Key != ConsoleKey.Spacebar)
        {
            Console.WriteLine("Press SPACE to spin!");
            keyInfo = Console.ReadKey();
        }

        int boxToSpin = _grid.GetRandomBoxToSpin();
        _grid.GetBoxes()[boxToSpin].OpenBox();
        _grid.PrintGrid();
    }

    private void InputBetAmount()
    {
        Console.WriteLine("How much would you like to bet?");
        int betAmount;
        bool validInput = int.TryParse(Console.ReadLine(), out betAmount);
        
        while (!validInput || betAmount <= 0)
        {
            Console.WriteLine("Please enter a valid bet amount");
            validInput = int.TryParse(Console.ReadLine(), out betAmount);
        }
        
        _betAmount = betAmount;
    }

    public void StartGame()
    {
        InputBetAmount();
        _grid.PrintGrid();
        
        while (_grid.CheckWin()==-1)
        {
            SpinBox();
            int winMultiplier = _grid.CheckWin();
            if (winMultiplier != -1)
            {
                Console.WriteLine($"You won {winMultiplier}x your bet!");
                Console.WriteLine($"You won {winMultiplier * _betAmount}!");
                break;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        SafeCrackerGame game = new SafeCrackerGame();
        game.StartGame();
    }
}