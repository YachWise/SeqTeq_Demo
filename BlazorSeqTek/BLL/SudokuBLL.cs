using System.Text.Json;

public class SudokuService
{
    private const string SessionKey = "SudokuGameState";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SudokuService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext?.Session ?? throw new InvalidOperationException("Session Unavailable.");

    public SudokuGame GetGame()
    {
        try
        {
            if (Session != null && Session.TryGetValue(SessionKey, out var gameData))
            {
                //warning for possible null return, but it's handled
                return JsonSerializer.Deserialize<SudokuGame>(gameData);
            }

           
        }
        catch
        {
            throw new Exception("ruh roh raggy. -- GetGame()");
        }

        var newGame = SudokuGame.GenerateNewGame();
        //SaveGame(newGame);
        return newGame;
    }

    public void SaveGame(SudokuGame game)
    {
        try
        {
            if (Session != null)
            {
                //Session.Set(SessionKey, JsonSerializer.SerializeToUtf8Bytes(game));
            }
        }
        catch
        {
            throw new Exception("ruh roh raggy! --  SaveGame()");
        }
    }

    public void ResetGame()
    {
        try
        {
            if (Session != null)
            {
                Session.Remove(SessionKey);
            }
        }
        catch
        {
            throw new Exception("ruh roh raggy! -- ResetGame()");
        }
       
    }
}

public class SudokuGame
{
    //[,] multidimensional array. Hadn't had to use that in a very long time lol.
    public int?[,] Board { get; set; } = new int?[9, 9];
    public int?[,] Solution { get; set; } = new int?[9, 9];
    public bool[,] ImmutableCells { get; set; } = new bool[9, 9];
    public bool IsCompleted { get; set; }
    
    public static SudokuGame GenerateNewGame()
    {
        var game = new SudokuGame();
        GenerateSolution(game.Solution);
        //google said a medium difficulty puzzle would remove 40 numbers from a 9x9 matrix
        CreatePuzzle(game, 40);
        return game;
    }

    private static void GenerateSolution(int?[,] grid, int row = 0, int col = 0)
    {
        //use case handling
        if (row == 9)
        {
            return;
        }
        if (col == 9)
        {
            GenerateSolution(grid, row + 1, 0);
            return;
        }

        var nums = Enumerable.Range(1, 9).OrderBy(x => Guid.NewGuid()).ToList();
        foreach (var num in nums)
        {
            if (IsValid(grid, row, col, num))
            {
                grid[row, col] = num;
                GenerateSolution(grid, row, col + 1);
                if (grid[8, 8] != null) return;
                grid[row, col] = null;
            }
        }
    }

    private static bool IsValid(int?[,] grid, int row, int col, int num)
    {
        //check if the row is a valid solution
        for (int i = 0; i < 9; i++)
        {
            if (grid[row, i] == num) return false;
        }
        //check if the column is a valid solution
        for (int i = 0; i < 9; i++)
        {
            if (grid[i, col] == num) return false;
        }
        int subRow = row - row % 3;
        int subCol = col - col % 3;
        //this is kind of hardcode-y and lazy for specifically a 3x3 matrix solution 
        for (int i = subRow; i < subRow + 3; i++)
        {
            for (int j = subCol; j < subCol + 3; j++)
            {
                if (grid[i, j] == num) return false;
            }
        }
        //chbeck if the box is a valid solution

        //if makes through all checks, return true
        return true;
    }

    private static void CreatePuzzle(SudokuGame game, int cellsToRemove)
    {
        //this is working by creating a solved board and removing the cells from the completed puzzle 
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                game.Board[i, j] = game.Solution[i, j];
                game.ImmutableCells[i, j] = true;
            }
        }


        var random = new Random();
        int removed = 0;
        while (removed < cellsToRemove)
        {
            int row = random.Next(9);
            int col = random.Next(9);
            if (game.Board[row, col] != null)
            {
                game.Board[row, col] = null;
                game.ImmutableCells[row, col] = false;
                removed++;
            }
        }
    }

    public bool CheckCompletion()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (Board[i, j] != Solution[i, j])
                {
                    return false;
                }
            }
        }
        //if makes through loops, must be true, return true. 
        IsCompleted = true;
        return true;
    }
}

