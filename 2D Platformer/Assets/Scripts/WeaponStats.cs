using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour {

    public static WeaponStats instance;

    public int baseDamage = 20;
    [SerializeField] private int _damage;
    public int Damage {
        get { return _damage; }
        set { _damage = value; }
    }

    public float baseFireRate = 5;
    [SerializeField] private float _fireRate;
    public float FireRate {
        get { return _fireRate; }
        set { _fireRate = value; }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
}
