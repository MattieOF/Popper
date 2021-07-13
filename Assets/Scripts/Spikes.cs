using UnityEngine;

public class Spikes : MonoBehaviour
{
    public AudioClip impactSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameGlobals.alive && collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<Player>().Die((transform.position - collision.transform.position).normalized);
            Destroy(AudioUtil.PlaySoundAtPos(collision.transform.position, impactSound), 1f);
        }
    }
}
