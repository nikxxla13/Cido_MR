using UnityEngine;
using System.Collections.Generic;

public class TreeManager : MonoBehaviour
{
    [Header("Fruit Prefabs")]
    public GameObject applePrefab;
    public GameObject pearPrefab;
    public GameObject orangePrefab;

    [Header("Spawn Points")]
    public Transform[] lowSpawnPoints;
    public Transform[] midSpawnPoints;
    public Transform[] highSpawnPoints;

    private List<GameObject> activeFruits = new List<GameObject>();

    void Start() => SpawnFruitsForStage(0, false, 0f);

    public void SetStage(int stage, bool move, float swaySpeed)
    {
        RefreshTree();
        SpawnFruitsForStage(stage, move, swaySpeed);
    }

    public void RefreshTree()
    {
        foreach (var f in activeFruits)
            if (f != null) Destroy(f);
        activeFruits.Clear();
    }

    void SpawnFruitsForStage(int stage, bool move, float swaySpeed)
    {
        Transform[] spawnPoints = stage <= 1 ? lowSpawnPoints : stage <= 3 ? midSpawnPoints : highSpawnPoints;
        GameObject[] prefabs    = { applePrefab, pearPrefab, orangePrefab };

        foreach (var point in spawnPoints)
        {
            if (point == null) continue;
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            if (prefab == null) continue;

            GameObject fruit = Instantiate(prefab, point.position, point.rotation, point);
            activeFruits.Add(fruit);

            if (move)
                fruit.AddComponent<FruitSway>().speed = swaySpeed;
        }
    }
}
