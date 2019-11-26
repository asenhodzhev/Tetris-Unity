using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    // Игрово поле
    public static int w = 10;
    public static int h = 20;

    public static Transform[,] grid = new Transform[w, h];

    // Метод за закръгляне на координатите
    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    // проверка дали координатите са в границата
    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < w &&
                (int)pos.y >= 0);
    }

    // изтриване на ред, ако той бъде запълнен
    public static void deleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // изместване на редовете при изтрит ред
    public static void decreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                // изместване с 1 на долу
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // ъпдейтване на нова позиция
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // изместване на всички горни редове
    public static void decreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
            decreaseRow(i);
    }

    // проверка дали даден ред е пълен
    public static bool isRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    // изтриване на пъллни редове
    public static void deleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                --y;
            }
        }
    }
}
