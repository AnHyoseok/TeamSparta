using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public int zombiesPerWave = 5;
    public float spawnInterval = 1f;
    private int currentWave = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartWave();
        }
    }

    public void StartWave()
    {
        currentWave++;
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        for (int i = 0; i < zombiesPerWave; i++)
        {
           
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

           
            Vector3 spawnOffset = new Vector3(
                Random.Range(-0.3f, 0.3f),  
                Random.Range(-0.1f, 0.2f), 
                0f
            );

            Vector3 spawnPos = spawnPoint.position + spawnOffset;

        
            GameObject newZombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
            newZombie.transform.SetParent(null);
          
            string zombieLayerName = spawnPoint.name switch
            {
                "SpawnPoint1" => "Zombie1",
                "SpawnPoint2" => "Zombie2",
                "SpawnPoint3" => "Zombie3",
                _ => "Default"
            };

            int layerIndex = LayerMask.NameToLayer(zombieLayerName);
            if (layerIndex != -1)
            {
                newZombie.layer = layerIndex;
            }
            else
            {
                Debug.LogWarning($"잘못된 Layer 이름: {zombieLayerName}");
            }

            yield return new WaitForSeconds(Random.Range(0.4f, 0.7f)); 
        }
    }

}
