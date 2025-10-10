using UnityEngine;

public class TurretController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedTurret = Instantiate(prefab);
        spawnedTurret.transform.position = transform.position;
        spawnedTurret.GetComponent<ProjectileWepBeh>().DirectionCheck(pm.moveDir);

    }

}
