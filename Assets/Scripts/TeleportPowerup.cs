using UnityEngine;

public class TeleportPowerup : MonoBehaviour
{
    public float     powerupLength = 20;
    public float     healAmount    = 1;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameGlobals.alive && collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            player.EnableTP(powerupLength);
            player.Heal(healAmount);
            player.CollectedBubble();
            Destroy(AudioUtil.PlaySoundAtPos(collision.transform.position, pickupSound), 1f);
            Destroy(gameObject);
        }
    }
}
