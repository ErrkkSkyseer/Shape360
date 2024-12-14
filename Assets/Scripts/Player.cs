using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    GameManager gm;
    SpriteRenderer sr;
    Health health;

    InputAction attackInput;

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] int damage;
    [SerializeField] float fireRate;
    float fireTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = gameObject.GetComponent<Health>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        attackInput = InputSystem.actions.FindAction("Attack");

        health.OnTakeDamage += OnTakeDamage;
        health.OnDead += OnDead;

        gm = GameManager.Instance;

        GameManager.Instance.OnGameStart += OnGameStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (!health.IsAlive() || gm.IsGameOver)
            return;

        LookAtMouse();
        if (attackInput.IsPressed() && fireTimer <= 0)
        {
            SpawnBullet();
            fireTimer = 1/fireRate;
        }
        else
        {
            fireTimer-=Time.deltaTime;
        }
    }


    void SpawnBullet()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet rBullet = bulletGO.GetComponent<Bullet>();
        rBullet.Damage = damage;
    }

    private void OnDestroy()
    {
        health.OnTakeDamage -= OnTakeDamage;
        health.OnTakeDamage -= OnDead;

        GameManager.Instance.OnGameStart -= OnGameStart; 
    }

    void OnTakeDamage()
    {
        StartCoroutine(Flash(Color.yellow, 0.5f));
    }

    IEnumerator Flash (Color color, float duration)
    {
        sr.color = color;
        yield return new WaitForSeconds(duration);
        sr.color = Color.white; 
    }
    void OnDead()
    {
        sr.color = Color.red;
        StopAllCoroutines();
        GameManager.Instance.GameOver();
    }

    void OnGameStart()
    {
        sr.color= Color.white;
        health.Reset();
    }
    private void LookAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
