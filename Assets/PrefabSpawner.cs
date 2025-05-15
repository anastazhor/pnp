using UnityEngine;
using System.Collections.Generic;

public class PrefabSpawner : MonoBehaviour
{
    [Header("���������")]
    public GameObject prefabToSpawn;
    public int requiredObjects = 7;
    public List<Transform> spawnPoints = new List<Transform>();

    private List<GameObject> activeCollectibles = new List<GameObject>();

    public void StartSpawning()
    {
        // ������� ���������� �������
        ClearAllCollectibles();

        // ��������� ���������� �� ����� ������
        if (spawnPoints.Count < requiredObjects)
        {
            Debug.LogError($"����� {requiredObjects} �����, ������� {spawnPoints.Count}");
            return;
        }

        // ������� ��� ������� �����
        for (int i = 0; i < requiredObjects; i++)
        {
            CreateCollectible(spawnPoints[i].position);
        }
    }

    void CreateCollectible(Vector3 position)
    {
        GameObject obj = Instantiate(prefabToSpawn, position, Quaternion.identity);
        obj.name = $"Collectible_{activeCollectibles.Count + 1}";

        var collectible = obj.GetComponent<CollectibleItem>() ?? obj.AddComponent<CollectibleItem>();
        collectible.Initialize(this);

        activeCollectibles.Add(obj);
    }

    public void RegisterCollect(GameObject collectedObject)
    {
        activeCollectibles.Remove(collectedObject);
        Debug.Log($"�������� �������: {activeCollectibles.Count}");

        if (activeCollectibles.Count == 0)
        {
            Debug.Log("��� �������� �������!");
            QuestManager.Instance?.CompleteQuest();
        }
    }

    void ClearAllCollectibles()
    {
        foreach (var obj in activeCollectibles)
        {
            if (obj != null) Destroy(obj);
        }
        activeCollectibles.Clear();
    }
}