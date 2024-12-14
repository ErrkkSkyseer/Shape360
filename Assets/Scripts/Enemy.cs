using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent (typeof(Rigidbody2D),typeof(Health))]
public abstract class Enemy : MonoBehaviour
{
    #region Hidden

    GameManager gm;

    protected Rigidbody2D rb;
    protected Health health;

    [SerializeField] protected int damage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float spinSpeed;

    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();

        health.OnTakeDamage += OnTakeDamage;
        health.OnDead += OnDead;

        gm = GameManager.Instance;

        gm.OnGameStart += OnGameStart;
    }

    virtual protected void FixedUpdate()
    {
        if (!gm.IsGameOver)
            Move();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerCollided(collision.gameObject);
        }
    }

    private void OnDestroy()
    {
        health.OnTakeDamage -= OnTakeDamage;
        health.OnDead -= OnDead;

        gm.OnGameStart -= OnGameStart;

    }
    #endregion

    protected abstract void Move();
    protected abstract void OnTakeDamage();

    #region Hidden

    virtual protected void OnDead()
    {
        if (!gm.IsGameOver)
            gm.IncreaseKillCount();
        Destroy(gameObject);
    }

    void OnGameStart()
    {
        Destroy(gameObject);
    }
    protected Vector2 GetOriginUnitVector()
    {
        Vector2 dir = Vector2.zero - (Vector2)transform.position;
        return dir.normalized;
    }

    protected Vector2 Rotate90(Vector2 v, char direction)
    {
        if (direction == 'l')
            return new Vector2(-v.y, v.x);
        else if (direction == 'r')
            return new Vector2(v.y, -v.x);

        return v;
    }
    protected Quaternion LookAt(Vector3 target)
    {
        Vector2 distance = target - transform.position;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnPlayerCollided(GameObject player)
    {
        Health pHealth = player.GetComponent<Health>();
        pHealth.TakeDamage(damage);
        health.Kill();
    }
    #endregion
}
