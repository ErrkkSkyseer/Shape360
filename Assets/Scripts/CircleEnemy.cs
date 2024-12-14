using UnityEngine;

public class CircleEnemy : Enemy
{
    char side;
    protected override void Start()
    {
        base.Start();
        side = Random.Range(0, 2)==0? 'l':'r' ;
    }
    protected override void Move()
    {
        rb.linearVelocity = 
            GetOriginUnitVector() * moveSpeed + 
            Rotate90(GetOriginUnitVector(),side) * spinSpeed;
    }

    protected override void OnTakeDamage()
    {

    }
}
