using UnityEngine;

public class PowerupUpgrade : MonoBehaviour
{
    public float healAmount = 1f;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameGlobals.alive && collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            player.CollectedBubble();
            player.powerupUpgrade++;
            player.Heal(healAmount);
            Destroy(AudioUtil.PlaySoundAtPos(collision.transform.position, pickupSound), 1f);
            Destroy(gameObject);
        }
    }
}
