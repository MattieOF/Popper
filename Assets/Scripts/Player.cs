using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerEyeReferences
    {
        public PlayerEye eye;
        public SpriteRenderer spriteRenderer;

        public void Reset(Sprite playerEyeSprite)
        {
            spriteRenderer.transform.localScale = Vector3.one;
            spriteRenderer.sprite = playerEyeSprite;
            spriteRenderer.color = Color.white;
            eye.dead = false;
        }
    }
    
    [Header("Scene References")]
    public PlayerController playerController;

    [Header("Asset References")] 
    public Sprite playerEyeSprite;
    public Sprite crossSprite;

    [Header("Player State")]
    public float health = 1f;
    public float healthLossRate = 0.25f;
    public PlayerEyeReferences[] eyes;
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

    private float _maxHealth;
    private float _timeSinceLastBubble = 0;
    private float _damageDownTime = 0;
    private float _tpTime = 0;

    public bool DamageDownActive => _damageDownTime > 0;

    public bool TpActive => _tpTime > 0;

    private void Start()
    {
        _maxHealth = health;
    }

    private void Update()
    {
        if (dead) return;

        if (!playerController.Grounded || _timeSinceLastBubble > maxBubblelessTime)
            health -= healthLossRate * Time.deltaTime * (DamageDownActive ? 0.1f : 1f);

        if (health <= 0)
            Die(Vector3.zero);

        _timeSinceLastBubble += Time.deltaTime;

        if (DamageDownActive) _damageDownTime -= Time.deltaTime;
        if (TpActive) _tpTime -= Time.deltaTime;
        UpdatePowerupHUD();

        UpdateEyeColour();
        healthbar.SetValue(health);
    }

    public void EnableDamageDown(float time)
    {
        _damageDownTime = time + (powerupUpgrade * 5);
    }

    public void EnableTp(float time)
    {
        _tpTime = time + (powerupUpgrade * 2);
    }

    public void UpdatePowerupHUD()
    {
        if (DamageDownActive)
        {
            damageDownObject.SetActive(true);
            damageDownTimerText.text = $"{Mathf.RoundToInt(_damageDownTime)}s";
        }
        else
            damageDownObject.SetActive(false);

        if (TpActive)
        {
            tpObject.SetActive(true);
            tpTimerText.text = $"{Mathf.RoundToInt(_tpTime)}s";
        }
        else
            tpObject.SetActive(false);
    }

    public void UpdateEyeColour()
    {
        Color newColour = Color.Lerp(Color.red, Color.white, health / _maxHealth);
        foreach (PlayerEyeReferences eye in eyes)
            eye.spriteRenderer.color = newColour;
    }
    
    public void CollectedBubble()
    {
        _timeSinceLastBubble = 0;
    }

    public void Die(Vector3 dir)
    {
        // Set state values
        health = 0;
        dead = true;
        healthbar.SetValue(0);
        
        // Update eyes
        foreach (PlayerEyeReferences eye in eyes)
        {
            eye.spriteRenderer.transform.localScale = Vector3.one * 0.5f;
            eye.spriteRenderer.sprite = crossSprite;
            eye.spriteRenderer.color = Color.red;
            eye.eye.dead = true;
        }

        // Reset powerup timers
        _damageDownTime = 0;
        _tpTime = 0;
        UpdatePowerupHUD();

        // Blood particles
        if (dir != Vector3.zero)
            Destroy(Instantiate(bloodParticle, playerHead.position, Quaternion.Euler(0, 0, Vector3.Angle(transform.position, dir))), 3f);
        else
            Destroy(Instantiate(bloodParticle, playerHead.position, Quaternion.identity), 3f);

        manager.GameOver();
    }

    public void Heal(float amount)
    {
        // Can't heal if dead
        if (dead) return;
        
        // Add to health and check ranges
        health += amount;
        if (health > _maxHealth) health = _maxHealth;
        if (health <= 0) Die(Vector3.zero);
        
        // Update HUD
        healthbar.SetValue(health);
    }

    public void Restart()
    {
        playerController.ResetTo(defaultPosition);
        _timeSinceLastBubble = 0;
        dead = false;
        health = 1;
        healthbar.SetValue(health);
        UpdateEyeColour();
        powerupUpgrade = 0;
        
        // Reset eyes
        foreach (PlayerEyeReferences eye in eyes)
            eye.Reset(playerEyeSprite);
    }
}
