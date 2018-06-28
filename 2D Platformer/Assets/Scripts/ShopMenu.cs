using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {

    public Transform pistol;
    public Transform rifle;

    // Health upgrade
    [SerializeField] private Text healthText;
    [SerializeField] private int healthUpgradeAmount = 20;
    [SerializeField] private int healthUpgradeCost = 40;
    //private int increaseHealthCostOnUpgradeAmount = 10;
    [SerializeField] private GameObject healthUpgradeButton;

    // Speed upgrade
    [SerializeField] private Text speedText;
    [SerializeField] private float speedUpgradeMultiplier = 1.2f;
    [SerializeField] private int speedUpgradeCost = 50;
    [SerializeField] private GameObject speedUpgradeButton;

    // Damage upgrade
    [SerializeField] private Text damageText;
    [SerializeField] private int damageUpgradeAmount = 10;
    [SerializeField] private int damageUpgradeCost = 50;
    [SerializeField] private GameObject damageUpgradeButton;

    // Extra Life
    [SerializeField] private Text numLivesText;
    [SerializeField] private int numLives;
    [SerializeField] private int extraLifeCost = 150;
    [SerializeField] private int numExtraLife = 1;  // Number of lives given when extraLife is purchased
    [SerializeField] private GameObject extraLifeBuyButton;

    // Rifle
    [SerializeField] private int rifleCost = 0;
    [SerializeField] private Image rifleImage;
    [SerializeField] private GameObject rifleBuyButton;

    [SerializeField] private GameObject soldOutButton;    // Reference to sold out button

    private int increaseCostOnBuyAmount = 10;
    
    private string upgradeSoundName = "Upgrade";
    private string insufficientGoldSoundName = "InsufficientGold";

    private PlayerStats stats;

    private void OnEnable() {
        stats = PlayerStats.instance;

        numLives = GameMaster.NumLives;

        UpdateValues();
    }

    void UpdateValues() {
        healthText.text = "HEALTH: " + stats.MaxHealth.ToString();
        speedText.text = "SPEED: " + stats.Velocity.ToString();
        damageText.text = "DAMAGE: " + WeaponStats.instance.Damage.ToString();
        numLivesText.text = "EXTRA LIFE: " + GameMaster.NumLives.ToString();
    }

    public void UpgradeHealth() {
        if (GameMaster.Gold < healthUpgradeCost) {
            AudioManager.instance.PlaySound(insufficientGoldSoundName);
            return;
        }

        stats.MaxHealth += healthUpgradeAmount;

        GameMaster.Gold -= healthUpgradeCost;
        AudioManager.instance.PlaySound(upgradeSoundName);

        // Increase cost
        healthUpgradeCost += increaseCostOnBuyAmount;

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

        // Increase cost
        speedUpgradeCost += increaseCostOnBuyAmount;

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

        // Increase cost
        damageUpgradeCost += increaseCostOnBuyAmount;

        UpdateValues();
    }

    public void BuyRifle() {
        if (GameMaster.Gold < rifleCost) {
            AudioManager.instance.PlaySound(insufficientGoldSoundName);
            return;
        }

        // Equip the rifle on the player
        Player.instance.weapon = Player.WeaponType.RIFLE;
        // Set gameobject active
        pistol.gameObject.SetActive(false);
        rifle.gameObject.SetActive(true);
        // Change weapon stats accordingly
        WeaponStats.instance.Damage = Weapon.instance.rifleBaseDamage;
        Debug.Log(WeaponStats.instance.Damage.ToString());
        WeaponStats.instance.FireRate = Weapon.instance.rifleBaseFireRate;
        Debug.Log(WeaponStats.instance.FireRate.ToString());

        GameMaster.Gold -= rifleCost;
        AudioManager.instance.PlaySound(upgradeSoundName);

        rifleBuyButton.SetActive(false);
        rifleImage.enabled = false;
        soldOutButton.SetActive(true);
    }

    public void BuyExtraLife() {
        if (GameMaster.Gold < extraLifeCost) {
            AudioManager.instance.PlaySound(insufficientGoldSoundName);
            return;
        }

        GameMaster.NumLives += numExtraLife;

        // Increase cost
        extraLifeCost += increaseCostOnBuyAmount;

        GameMaster.Gold -= extraLifeCost;
        AudioManager.instance.PlaySound(upgradeSoundName);
    }

    public void SoldOut() {
        AudioManager.instance.PlaySound(insufficientGoldSoundName);
    }
}
