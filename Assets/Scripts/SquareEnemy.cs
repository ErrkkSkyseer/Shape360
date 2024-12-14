using UnityEngine;

public class SquareEnemy : Enemy
{

    [SerializeField] float pushInterval = 1.0f;
    [SerializeField] float pushForce = 10f;

    [SerializeField] float distanceMul = 3;
    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(PushSide), 1f, pushInterval);
    }
    protected override void Move()
    {
        Spin();

        rb.AddForce(GetOriginUnitVector() * moveSpeed,ForceMode2D.Force);
    }

    private void Spin()
    {
        transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
    }

    void PushSide()
    {
        if(!IsObjectOnScreen(gameObject))
            return;

        Vector2 distance = Vector2.zero - (Vector2)transform.position;
        float applyPushForce = pushForce * distance.magnitude / distanceMul;

        if (UnityEngine.Random.value > 0.5)
            rb.AddForce(Rotate90(GetOriginUnitVector(),'l') * applyPushForce, ForceMode2D.Impulse);
        else
            rb.AddForce(Rotate90(GetOriginUnitVector(), 'r') * applyPushForce, ForceMode2D.Impulse);
    }

    public bool IsObjectOnScreen(GameObject target)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.transform.position);

        // Check if the object is within the screen bounds (width and height of the screen)
        return screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
               screenPoint.y >= 0 && screenPoint.y <= Screen.height;
    }

    protected override void OnTakeDamage()
    {

    }
}
