using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

    [SerializeField] private Text healthText;
    [SerializeField] private Text speedText;

    [SerializeField] private int healthUpgradeAmount = 20;
    [SerializeField] private int healthUpgradeCost = 40;

    [SerializeField] private float speedUpgradeMultiplier = 1.2f;
    [SerializeField] private int speedUpgradeCost = 50;

    [SerializeField] private int damageUpgradeAmount = 10;
    [SerializeField] private int damageUpgradeCost = 50;

    [SerializeField] private int rifleCost = 200;
    public GameObject rifleButton;

    public GameObject soldOutButton;

    private string upgradeSoundName = "Upgrade";
    private string insufficientGoldSoundName = "InsufficientGold";

    private PlayerStats stats;

    private void OnEnable() {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues() {
        healthText.text = "HEALTH: " + stats.MaxHealth.ToString();
        speedText.text = "SPEED: " + stats.Velocity.ToString();
    }

    public void UpgradeHealth() {
        if (GameMaster.Gold < healthUpgradeCost) {
            AudioManager.instance.PlaySound(insufficientGoldSoundName);
            return;
        }

        stats.MaxHealth += healthUpgradeAmount;

        GameMaster.Gold -= healthUpgradeCost;
        AudioManager.instance.PlaySound(upgradeSoundName);

        UpdateValues();
    }

    public void UpgradeSpeed() {
        if (GameMaster.Gold < healthUpgradeCost) {
            AudioManager.instance.PlaySound(insufficientGoldSoundName);
            return;
        }

        stats.Velocity = Mathf.Round(stats.Velocity * speedUpgradeMultiplier);

        GameMaster.Gold -= speedUpgradeCost;
        AudioManager.instance.PlaySound(upgradeSoundName);

        UpdateValues();
    }

    public void UpgradeDamage() {
        if (GameMaster.Gold < damageUpgradeCost) {
            AudioManager.instance.PlaySound(insufficientGoldSoundName);
            return;
        }

        // Add damageUpgradeAmount to weapon damage
        WeaponStats.instance.Damage += damageUpgradeAmount;
        Debug.Log(WeaponStats.instance.Damage);

        GameMaster.Gold -= damageUpgradeCost;
        AudioManager.instance.PlaySound(upgradeSoundName);

        UpdateValues();
    }

    public void BuyRifle() {
        if (GameMaster.Gold < rifleCost) {
            AudioManager.instance.PlaySound(insufficientGoldSoundName);
            return;
        }

        // Do Something
        Debug.Log("Purchased Rifle.");

        GameMaster.Gold -= rifleCost;
        AudioManager.instance.PlaySound(upgradeSoundName);

        rifleButton.SetActive(false);
        soldOutButton.SetActive(true);
    }

    public void SoldOut() {
        AudioManager.instance.PlaySound(insufficientGoldSoundName);
    }
}
