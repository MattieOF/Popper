using UnityEngine;

public class Spawn5 : MonoBehaviour
{
    public int spawnCount = 5;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameGlobals.alive && collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            player.CollectedBubble();
            Manager manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
            for (int i = 0; i < spawnCount + player.powerupUpgrade; i++)
            {
                manager.SpawnBubble();
            }
            Destroy(AudioUtil.PlaySoundAtPos(collision.transform.position, pickupSound), 1f);
            Destroy(gameObject);
        }
    }
}
