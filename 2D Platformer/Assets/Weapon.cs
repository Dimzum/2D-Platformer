using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0;
    public float dmg = 10;
    public LayerMask whatToHit;

    public Transform bulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    private float timeToFire = 0;
    private Transform firePoint;

    private void Awake() {
        firePoint = transform.Find("FirePoint");

        if (firePoint == null) {
            Debug.LogError("'firePoint' is null");
        }
    }

    // Update is called once per frame
    void Update () {
		if (fireRate == 0) {
            if (Input.GetButtonDown("Fire1")) {
                Shoot();
            }
        }
        else {
            if (Input.GetButton("Fire1") && Time.time > timeToFire) {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}

    void Shoot() {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector2 dir = mousePos - firePointPos;
        RaycastHit2D hit = Physics2D.Raycast(firePointPos, dir, 1000, whatToHit); // (mousePos - firePoint) to get the dir
        if (Time.time >= timeToSpawnEffect) {
            Effect();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
        //Debug.DrawLine(firePointPos, dir * 1000, Color.red);

        if (hit.collider != null) {
            //Debug.DrawLine(firePointPos, hit.point, Color.cyan);
            Debug.Log("We hit " + hit.collider.name + " for " + dmg + " damage.");
        }
    }

    void Effect() {
        Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);

        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint.parent;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);
    }
}
