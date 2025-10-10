using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ProjectileBeh : ProjectileWepBeh

{

    TurretController tc;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        tc = FindFirstObjectByType<TurretController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * tc.speed * Time.deltaTime;
    }
}
