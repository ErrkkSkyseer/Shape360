using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    int enemyKill;

    bool isGameOver = true;

    const int LEFT_BOUND = -18;
    const int LOWER_BOUND = -10;
    const int SCREEN_WIDTH = 36;
    const int SCREEN_HEIGHT = 20;

    [SerializeField] Rect screen;
    [SerializeField] Rect spawnableArea;

    [SerializeField] GameObject[] enemiePrefabs;

    [SerializeField] float spawnInterval;

    public UnityAction OnGameStart;
    public UnityAction OnGameEnd;

    public int EnemyKill { get => enemyKill; }
    public bool IsGameOver { get => isGameOver; }


    public void IncreaseKillCount() => enemyKill++;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        enemyKill = 0;
    }

    private void Update()
    {

    }
    
    #region SpawnEnemyOverload

    void SpawnEnemy()
    {
        Vector2 randomPosition = GetRandomPositionFromDifference(spawnableArea, screen);
        int randomEnemyIndex = Random.Range(0, enemiePrefabs.Length);
        Instantiate(enemiePrefabs[randomEnemyIndex], randomPosition, Quaternion.identity);
    }
    void SpawnEnemy(Vector2 position)
    {
        int randomEnemyIndex = Random.Range(0, enemiePrefabs.Length);
        Instantiate(enemiePrefabs[randomEnemyIndex], position, Quaternion.identity);
    }
    void SpawnEnemy(int index)
    {
        Vector2 randomPosition = GetRandomPositionFromDifference(spawnableArea, screen);
        Instantiate(enemiePrefabs[index], randomPosition, Quaternion.identity);
    }
    void SpawnEnemy(Vector2 position, int index)
    {
        Instantiate(enemiePrefabs[index], position, Quaternion.identity);
    }
    #endregion

    public void StartGame()
    {
        enemyKill = 0;
        InvokeRepeating(nameof(SpawnEnemy), 0, spawnInterval);
        isGameOver = false;
        OnGameStart?.Invoke();
    }

    public void GameOver()
    {
        CancelInvoke(nameof(SpawnEnemy));
        isGameOver = true;
        OnGameEnd?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, screen.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, spawnableArea.size);

    }

    // helper function - chat GPT
    public Vector2 GetRandomPositionFromDifference(Rect rectA, Rect rectB)
    {
        // Calculate the intersection of the two rectangles
        Rect intersection = Rect.MinMaxRect(
            Mathf.Max(rectA.xMin, rectB.xMin),  // Left side
            Mathf.Max(rectA.yMin, rectB.yMin),  // Bottom side
            Mathf.Min(rectA.xMax, rectB.xMax),  // Right side
            Mathf.Min(rectA.yMax, rectB.yMax)   // Top side
        );

        // If there is no intersection, return a random point from rectA
        if (intersection.width <= 0 || intersection.height <= 0)
        {
            return new Vector2(
                Random.Range(rectA.xMin, rectA.xMax),
                Random.Range(rectA.yMin, rectA.yMax)
            );
        }

        // Calculate the areas of the non-intersecting regions of rectA
        float leftWidth = intersection.xMin - rectA.xMin;
        float rightWidth = rectA.xMax - intersection.xMax;
        float bottomHeight = intersection.yMin - rectA.yMin;
        float topHeight = rectA.yMax - intersection.yMax;

        // Total area of non-intersecting regions
        float totalArea = leftWidth * rectA.height + rightWidth * rectA.height +
                          bottomHeight * rectA.width + topHeight * rectA.width;

        // Randomly select a region based on the area
        float randomPoint = Random.Range(0f, totalArea);

        // Check each region and return a random point from the selected region
        if (randomPoint < leftWidth * rectA.height)
        {
            return new Vector2(Random.Range(rectA.xMin, intersection.xMin), Random.Range(rectA.yMin, rectA.yMax));
        }
        randomPoint -= leftWidth * rectA.height;

        if (randomPoint < rightWidth * rectA.height)
        {
            return new Vector2(Random.Range(intersection.xMax, rectA.xMax), Random.Range(rectA.yMin, rectA.yMax));
        }
        randomPoint -= rightWidth * rectA.height;

        if (randomPoint < bottomHeight * rectA.width)
        {
            return new Vector2(Random.Range(rectA.xMin, rectA.xMax), Random.Range(rectA.yMin, intersection.yMin));
        }
        randomPoint -= bottomHeight * rectA.width;

        // Return a point in the top region
        return new Vector2(Random.Range(rectA.xMin, rectA.xMax), Random.Range(intersection.yMax, rectA.yMax));
    }
}
