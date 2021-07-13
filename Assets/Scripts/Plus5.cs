using UnityEngine;

public class Plus5 : MonoBehaviour
{
    public int       collectCount = 5;
    public float     healAmount   = 0.25f;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameGlobals.alive && collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            player.CollectedBubble();
            player.Heal(healAmount);
            Manager manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
            for (int i = 0; i < collectCount + player.powerupUpgrade; i++)
            {
                manager.AddBubble();
            }
            Destroy(AudioUtil.PlaySoundAtPos(collision.transform.position, pickupSound), 1f);
            Destroy(gameObject);
        }
    }
}
