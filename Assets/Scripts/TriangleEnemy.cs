using TMPro;
using UnityEngine;

public class TriangleEnemy : Enemy
{
    protected override void Move()
    {
        transform.rotation = LookAt(Vector3.zero);
        rb.linearVelocity = transform.up * moveSpeed;
    }

    protected override void OnTakeDamage()
    {

    }
}
