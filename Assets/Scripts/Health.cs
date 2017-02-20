using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Character))]
public class Health : MonoBehaviour
{
    [Header("Player")]
    public Transform healthBar;
    public Text healthText;
    [Header("NPC")]
    public GameObject healthBarPrefab;

    private CharacterStats stats;

    private float currentHealth;
    private float currentArmor;
    private float currentShield;

    public float lastDamageTime { get; private set; }

    private RectTransform healthSlider;
    private RectTransform armorSlider;
    private RectTransform shieldSlider;

	void Start ()
    {
        stats = GetComponent<Character>().stats;

        currentHealth = stats.maxHealth;
        currentArmor = stats.maxArmor;
        currentShield = stats.maxShield;

        if(transform.tag != "Player")
        {
            healthBar = ((GameObject)GameObject.Instantiate(healthBarPrefab, transform, false)).transform;
        }

        if (healthBar != null)
        {
            healthSlider = healthBar.Find("HealthBar/Bar/Health") as RectTransform;
            armorSlider = healthBar.Find("HealthBar/Bar/Armor") as RectTransform;
            shieldSlider = healthBar.Find("HealthBar/Bar/Shield") as RectTransform;

            UpdateHealth();
        }
    }

    void Update()
    {
        // Shield regen
        if(currentShield < stats.maxShield && Time.time > lastDamageTime + 3)
        {
            currentShield += 30 * Time.deltaTime;
            if (currentShield > stats.maxShield)
                currentShield = stats.maxShield;

            UpdateHealth();
        }
    }

    public void Damage(float value)
    {
        float damage = value;

        // Shield
        if(currentShield > damage)
        {
            currentShield -= damage;
            damage = 0;
        }
        else if(currentShield > 0)
        {
            damage -= currentShield;
            currentShield = 0;
        }

        // Armor
        if(currentArmor > 0)
        {
            float armorDamage = damage > 10 ? damage - 5 : damage / 2;

            if (currentArmor > armorDamage)
            {
                currentArmor -= armorDamage;
                damage = 0;
                armorDamage = 0;
            }
            else
            {
                armorDamage -= currentArmor;
                currentArmor = 0;
            }
            currentHealth -= armorDamage;
        }
        else // Health
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
            GameObject.Destroy(gameObject);

        UpdateHealth();

        lastDamageTime = Time.time;
    }

    public void Heal(float value)
    {
        float excess = 0;
        currentHealth += value;
        if (currentHealth > stats.maxHealth)
        {
            excess = currentHealth - stats.maxHealth;
            currentHealth = stats.maxHealth;
        }
        if (stats.maxArmor > 0)
        {
            currentArmor += excess;
            excess = 0;
            if (currentArmor > stats.maxArmor)
            {
                excess = currentArmor - stats.maxArmor;
                currentArmor = stats.maxArmor;
            }
        }
        if (stats.maxShield > 0)
        {
            currentShield += excess;
            excess = 0;
            if (currentShield > stats.maxShield)
            {
                excess = currentShield - stats.maxShield;
                currentShield = stats.maxShield;
            }
        }


        UpdateHealth();
    }
    float max;
    float currentTotal;

    private void UpdateHealth()
    {
        if (healthBar == null)
            return;

        max = (currentHealth > stats.maxHealth ? currentHealth : stats.maxHealth) + (currentArmor > stats.maxArmor ? currentArmor : stats.maxArmor) + (currentShield > stats.maxShield ? currentShield : stats.maxShield);
        if (max == 0)
        {
            SetAnchor(healthSlider, 0, 0);
            SetAnchor(armorSlider, 0, 0);
            SetAnchor(shieldSlider, 0, 0);
        }
        else
        {
            currentTotal = currentHealth + currentArmor + currentShield;

            SetAnchor(healthSlider, 0, (currentHealth / max));
            SetAnchor(armorSlider, healthSlider.anchorMax.x, healthSlider.anchorMax.x + (currentArmor / max));
            SetAnchor(shieldSlider, armorSlider.anchorMax.x, armorSlider.anchorMax.x + (currentShield / max));
        }

        if (healthText != null)
            healthText.text = string.Format("{0} / {1}", (int)currentTotal, max);
    }
    private void SetAnchor(RectTransform rt, float min, float max)
    {
        rt.anchorMin = new Vector2(min, rt.anchorMin.y);
        rt.anchorMax = new Vector2(max, rt.anchorMax.y);
    }
}
