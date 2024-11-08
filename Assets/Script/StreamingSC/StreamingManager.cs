using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StreamingManager : MonoBehaviour
{

    //���� ���� ... 10m �� ��ǥ�� 100�̴�. 10��...
    public Transform player; // �÷��̾� ĳ������ Transform
    public int sectorSize = 10; // �� ������ ũ�� (����: ����)
    public int loadDistance = 1; // �÷��̾�� �� �� ���� �̳��� �Ÿ����� �ε����� ����
    
    private Dictionary<Vector2Int, string> loadedSectors = new Dictionary<Vector2Int, string>();

    void Update()
    {
        Vector2Int currentSector = GetSector(player.position);
        Debug.Log($"Player Position: {player.position}, Current Sector: {currentSector}");
        // �ֺ� ���� �ε�
        for (int x = -loadDistance; x <= loadDistance; x++)
        {
            for (int y = -loadDistance; y <= loadDistance; y++)
            {
                Vector2Int sectorToLoad = currentSector + new Vector2Int(x, y);

                //�ش� ���Ͱ� �ε� �Ǿ����� �ʴٸ� �ε��ϱ�.
                if (!loadedSectors.ContainsKey(sectorToLoad))
                {
                    //Debug.Log($"Loading Sector: {sectorToLoad}");
                    //StartCoroutine(LoadSector(sectorToLoad));
                }
            }
        }

        // �ָ� �ִ� ���� ��ε�
        List<Vector2Int> sectorsToUnload = new List<Vector2Int>();
        foreach (var sector in loadedSectors.Keys)
        {
            if (Vector2Int.Distance(sector, currentSector) > loadDistance)
            {
               // sectorsToUnload.Add(sector);
            }
        }
        foreach (var sector in sectorsToUnload)
        {
           // StartCoroutine(UnloadSector(sector));
        }
    }

    Vector2Int GetSector(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / sectorSize);
        int y = Mathf.FloorToInt(position.z / sectorSize);

        return new Vector2Int(x, y);
    }

    IEnumerator LoadSector(Vector2Int sector)
    {
        string sceneName = $"Sector_{sector.x}_{sector.y}";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        loadedSectors.Add(sector, sceneName);
    }

    IEnumerator UnloadSector(Vector2Int sector)
    {
        if (loadedSectors.TryGetValue(sector, out string sceneName))
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
            loadedSectors.Remove(sector);
        }
    }
}
