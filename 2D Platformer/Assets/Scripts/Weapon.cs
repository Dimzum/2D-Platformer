using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0;
    public int damage = 10;
    public LayerMask whatToHit;

    public Transform bulletTrailPrefab;
    public Transform hitPrefab;
    public Transform muzzleFlashPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    // Handle camera shaking
    public float camShakeAmount = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake camshake;

    private float timeToFire = 0;
    private Transform firePoint;

    private void Awake() {
        firePoint = transform.Find("FirePoint");

        if (firePoint == null) {
            Debug.LogError("'firePoint' is null");
        }
    }

    private void Start() {
        camshake = GameMaster.gm.GetComponent<CameraShake>();
        if (camshake == null) {
            Debug.LogError("No camera shake found on GM object.");
        }
    }

    // Update is called once per frame
    void Update () {
		if (fireRate == 0) {
            if (Input.GetButtonDown("Fire1")) {
                Shoot();
            }
        } else {
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
        
        //Debug.DrawLine(firePointPos, dir * 1000, Color.red);
        if (hit.collider != null) {
            //Debug.DrawLine(firePointPos, hit.point, Color.cyan);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null) {
                //Debug.Log("We hit " + hit.collider.name + " for " + damage + " damage.");
                enemy.DamageEnemy(damage);
            }
        }

        if (Time.time >= timeToSpawnEffect) {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null) {
                hitPos = (mousePos - firePointPos) * 30;
                hitNormal = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            } else {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos, hit.normal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal) {
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null) {
            lr.SetPosition(0, firePoint.position); // Starting pos
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.04f);

        if (hitNormal != new Vector3(float.MaxValue, float.MaxValue, float.MaxValue)) {
            Transform hitParticle = Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.up, hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint.parent;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);

        // Shake the camera
        //camshake.Shake(camShakeAmount, camShakeLength);
    }
}
