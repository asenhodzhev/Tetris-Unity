using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Групи
    public GameObject[] groups;

    public void spawnNext()
    {
        // Произволен индекс
        int i = Random.Range(0, groups.Length);

        // Поставяне на обекта на определената позиция
        Instantiate(groups[i],
                    transform.position,
                    Quaternion.identity);
    }

    void Start()
    {
        // Извикване на метода при стартиране на играта
        spawnNext();
    }
}
