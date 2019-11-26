using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    float lastFall = 0;

    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);

            // дали не е в границата
            if (!Playfield.insideBorder(v))
                return false;

            // дали има блок в клетката на грида и не е част от същата група
            if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                Playfield.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    void updateGrid()
    {
        // премахва старата позиция в грида
        for (int y = 0; y < Playfield.h; ++y)
            for (int x = 0; x < Playfield.w; ++x)
                if (Playfield.grid[x, y] != null)
                    if (Playfield.grid[x, y].parent == transform)   
                        Playfield.grid[x, y] = null;

        // добавя новата позиция
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    void Start()
    {
        // Ако не е валидна дефоутната позиция играта приключва
        if (!isValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }

 
    void Update()
    {
        // Движение наляво
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // смяна на позицията
            transform.position += new Vector3(-1, 0, 0);

            // проверка дали е валидна
            if (isValidGridPos())
                // ако е валидна се ъпдейтва грида
                updateGrid();
            else
                // ако не е, се връща
                transform.position += new Vector3(1, 0, 0);
        }

        // движение надявно
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // смяна на позицията
            transform.position += new Vector3(1, 0, 0);

            // проверка дали е валидна
            if (isValidGridPos())
                // ако е валидна се ъпдейтва грида
                updateGrid();
            else
                // ако не е, се връща
                transform.position += new Vector3(-1, 0, 0);
        }

        // Ротация
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);

            // проверка дали е валидна
            if (isValidGridPos())
                // ако е валидна се ъпдейтва грида
                updateGrid();
            else
                // ако не е, се връща
                transform.Rotate(0, 0, 90);
        }

        // движение надолу и падане
        else if (Input.GetKeyDown(KeyCode.DownArrow) ||
                 Time.time - lastFall >= 1)
        {
            // смяна на позицията
            transform.position += new Vector3(0, -1, 0);

            // проверка дали е валидна
            if (isValidGridPos())
            {
                // ако е валидна се ъпдейтва грида
                updateGrid();
            }
            else
            {
                // ако не е, се връща
                transform.position += new Vector3(0, 1, 0);

                // изтриване на запълнени редове
                Playfield.deleteFullRows();

                // пускане на нова група
                FindObjectOfType<Spawner>().spawnNext();

                // спиране на дживението
                enabled = false;
            }

            lastFall = Time.time;
        }
    }
}