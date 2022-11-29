namespace Task.Application.Common.Extensions;

public static class MatrixExtension
{
    public static T[,] ConvertToMatrix<T>(this IEnumerable<T> sequence, int size)
    {
        var matrix = new T[size, size];
        var index = 0;

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                matrix[i, j] = sequence.ElementAt(index);
                index++;
            }
        }

        return matrix;
    }

    public static T[] GetColumn<T>(this T[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
            .Select(x => matrix[x, columnNumber])
            .ToArray();
    }

    public static T[] GetRow<T>(this T[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
            .Select(x => matrix[rowNumber, x])
            .ToArray();
    }

    public static T[] GetDiagonal<T>(this T[,] matrix, bool isPrincipalDiagonal = true)
    {
        var size = matrix.GetLength(1);
        var index = 0;
        var sequence = new T[size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                if (i == j && isPrincipalDiagonal)
                {
                    sequence[index] = matrix[i, j];
                    index++;
                }

                if (i + j == size - 1 && !isPrincipalDiagonal)
                {
                    sequence[index] = matrix[i, j];
                    index++;
                }
            }
        }

        return sequence;
    }
}