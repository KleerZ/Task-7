using Task.Application.Common.Data;
using Task.Application.Common.Extensions;

namespace Task.Application.Common.Game;

public class TicTacToe
{
    private const int Size = 3;

    public string? GetWinnerSymbol(string[] field)
    {
        if (field.All(e => e != string.Empty))
            return Winners.Draw;
        
        var map = field.ConvertToMatrix(Size);

        var results = new List<string?>
        {
            SearchVerticalWinner(map),
            SearchHorizontalWinner(map),
            SearchDiagonalWinner(map)
        };

        var winner = results.FirstOrDefault(r => r != null);

        return winner;
    }

    private string? SearchVerticalWinner(string[,] map)
    {
        for (var i = 0; i < Size; i++)
        {
            var column = map.GetColumn(i);
            var winner = GetSequenceWinner(column);

            if (winner != null)
                return winner;
        }

        return null;
    }

    private string? SearchHorizontalWinner(string[,] map)
    {
        for (var i = 0; i < Size; i++)
        {
            var row = map.GetRow(i);
            var winner = GetSequenceWinner(row);
            
            if (winner != null)
                return winner;
        }

        return null;
    }
    
    private string? SearchDiagonalWinner(string[,] map)
    {
        var principalDiagonal = map.GetDiagonal();
        var secondaryDiagonal = map.GetDiagonal(false);

        var winner = GetSequenceWinner(principalDiagonal);
        if (winner != null)
            return winner;

        winner = GetSequenceWinner(secondaryDiagonal);

        return winner;
    }
    
    private string? GetSequenceWinner(string[] sequence)
    {
        if (sequence.All(e => e == GameSymbols.Cross))
            return GameSymbols.Cross;
        if (sequence.All(e => e == GameSymbols.Zero))
            return GameSymbols.Zero;

        return null;
    }
}