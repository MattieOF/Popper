using UnityEngine;

public class Bubble : MonoBehaviour
{
    public  float       bounceSpeed     = 2f;
    public  float       bounceHeight    = 2f;
    public  float       healAmount      = 0.4f;
    public  float       autoDestroyTime = 45f;
    public  SoundGroup  popSounds;
    public  GameObject  popParticle;

    private Manager     manager;

    private void Start()
    {
        if (GameGlobals.inMainMenu) return;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        Invoke("Pop", autoDestroyTime);
    }

    private void Update()
    {
        float newY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + newY, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameGlobals.alive && collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            player.Heal(healAmount);
            player.CollectedBubble();
            Pop();
            manager.AddBubble();
        }
    }

    public void Pop()
    {
        Destroy(AudioUtil.PlaySoundAtPos(transform.position, popSounds), 1);
        Destroy(Instantiate(popParticle, transform.position, Quaternion.identity), 1);

        CancelInvoke();
        Destroy(gameObject);
    }
}
