using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats instance;

    [SerializeField] private int _maxHealth = 100;
    public int MaxHealth {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField] private int _currHealth;
    public int CurrHealth {
        get { return _currHealth; }
        set { _currHealth = Mathf.Clamp(value, 0, _maxHealth); }
    }

    public float healthRegenRate = 2f;

    [SerializeField] private float _velocity = 10f;    // The fastest the player can travel in the x axis.
    public float Velocity {
        get { return _velocity; }
        set { _velocity = value; }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
}