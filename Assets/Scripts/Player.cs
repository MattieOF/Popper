using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public PlayerController playerController;

    public float health = 1f;
    public float healthLossRate = 0.25f;
    public SpriteRenderer[] eyes;
    public Healthbar healthbar;
    public Manager manager;
    public bool dead = false;
    public Vector3 defaultPosition;
    public float maxBubblelessTime = 15f;
    public GameObject bloodParticle;
    public Transform playerHead;
    public int powerupUpgrade = 0;

    [Header("Powerup HUD")]
    public GameObject damageDownObject;
    public TextMeshProUGUI damageDownTimerText;
    public GameObject tpObject;
    public TextMeshProUGUI tpTimerText;

    private float maxHealth;
    private float timeSinceLastBubble = 0;
    private float damageDownTime = 0;
    private float tpTime = 0;

    public bool DamageDownActive
    {
        get { return damageDownTime > 0; }
    }

    public bool TPActive
    {
        get { return tpTime > 0; }
    }

    private void Start()
    {
        maxHealth = health;
    }

    private void Update()
    {
        if (dead) return;

        if (!playerController.Grounded || timeSinceLastBubble > maxBubblelessTime)
            health -= healthLossRate * Time.deltaTime * (DamageDownActive ? 0.1f : 1f);

        if (health <= 0)
            Die(Vector3.zero);

        timeSinceLastBubble += Time.deltaTime;

        if (DamageDownActive) damageDownTime -= Time.deltaTime;
        if (TPActive) tpTime -= Time.deltaTime;
        UpdatePowerupHUD();

        UpdateEyeColour();
        healthbar.SetValue(health);
    }

    public void EnableDamageDown(float time)
    {
        damageDownTime = time + (powerupUpgrade * 5);
    }

    public void EnableTP(float time)
    {
        tpTime = time + (powerupUpgrade * 2);
    }

    public void UpdatePowerupHUD()
    {
        if (DamageDownActive)
        {
            damageDownObject.SetActive(true);
            damageDownTimerText.text = $"{Mathf.RoundToInt(damageDownTime)}s";
        }
        else
            damageDownObject.SetActive(false);

        if (TPActive)
        {
            tpObject.SetActive(true);
            tpTimerText.text = $"{Mathf.RoundToInt(tpTime)}s";
        }
        else
            tpObject.SetActive(false);
    }

    public void UpdateEyeColour()
    {
        Color newColour = Color.Lerp(Color.red, Color.white, health / maxHealth);
        foreach (SpriteRenderer sr in eyes)
            sr.color = newColour;
    }

    public void CollectedBubble()
    {
        timeSinceLastBubble = 0;
    }

    public void Die(Vector3 dir)
    {
        health = 0;
        dead = true;
        healthbar.SetValue(0);
        UpdateEyeColour();

        // Reset powerup timers
        damageDownTime = 0;
        tpTime = 0;
        UpdatePowerupHUD();

        if (dir != Vector3.zero)
            Destroy(Instantiate(bloodParticle, playerHead.position, Quaternion.Euler(0, 0, Vector3.Angle(transform.position, dir))), 3f);
        else
            Destroy(Instantiate(bloodParticle, playerHead.position, Quaternion.identity), 3f);

        manager.GameOver();
    }

    public void Heal(float amount)
    {
        if (dead) return;
        health += amount;
        if (health > maxHealth) health = maxHealth;
        if (health <= 0) Die(Vector3.zero);
        healthbar.SetValue(health);
    }

    public void Restart()
    {
        playerController.ResetTo(defaultPosition);
        timeSinceLastBubble = 0;
        dead = false;
        health = 1;
        healthbar.SetValue(health);
        UpdateEyeColour();
        powerupUpgrade = 0;
    }
}
