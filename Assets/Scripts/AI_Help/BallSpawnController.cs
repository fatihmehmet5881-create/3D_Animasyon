using UnityEngine;

public class BallSpawnController : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float spawnInterval = 2f;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnInterval <= spawnTimer)
        {
            SpawnBall();
            spawnTimer = 0f;
        }
    }

    private void SpawnBall()
    {
        if(ballPrefab != null) Instantiate(ballPrefab, transform.position, transform.rotation);
    }
}
