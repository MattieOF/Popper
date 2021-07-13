using UnityEngine;

public struct PlayerPos
{
    public Vector3 position;
    public Quaternion rotation;
}

public class PlayerController : MonoBehaviour
{
    public  Camera       cam;
    public  Transform    playerHead;
    public  LineRenderer forceUI;
    public  Rigidbody2D  rb;
    public  Player       player;
    public  AudioClip    tpSound;
    public  GameObject   jumpParticle;

    // public  PlayerPos    defaultPos;

    // public  float        headRotationLimit = 70f;

    private bool         cancelMove        = false;
    private bool         forceUIActive     = false;
    private Vector3      forceOrigin       = Vector3.zero;

    public bool Grounded
    {
        get
        {
            return Physics2D.OverlapBox(playerHead.position + (Vector3.down * 0.12f), new Vector2(0.1f, 1.2f), 0, LayerMask.GetMask("Default"));
        }
    }

    private void Start()
    {
        if (!forceUI)
            Debug.LogError("Force ui on player controller is null", gameObject);
    }

    private void Update()
    {
        if (GameGlobals.menuOpen) return;

        if (Input.GetMouseButton(1))
        {
            cancelMove = true;
        }
        else if (!cancelMove && Input.GetMouseButton(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (player.TPActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    rb.MovePosition(mousePos);
                    rb.velocity = Vector2.zero;
                    Destroy(AudioUtil.PlaySoundAtPos(transform.position, tpSound), 1f);
                }
            } else
            {
                forceUIActive = true;
                if (Input.GetMouseButtonDown(0) || forceOrigin == null) forceOrigin = mousePos;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0)) Shoot();
            if (!Input.GetMouseButton(0)) cancelMove = false;
            DisableForceUI();
        }

        if (forceUIActive)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            forceUI.SetPosition(0, forceOrigin.WithZ(0));
            forceUI.SetPosition(1, mousePos.WithZ(0));
        }
    }

    public void DisableForceUI()
    {
        if (!forceUIActive) return;
        forceUIActive = false;
        forceUI.SetPosition(0, Vector3.zero);
        forceUI.SetPosition(1, Vector3.zero);
    }

    public void Shoot()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        float force = Vector3.Distance(forceOrigin, mousePos);
        Vector3 dir = (mousePos.WithZ(0) - forceOrigin.WithZ(0)).normalized;
        Debug.Log($"Launching with a force of {force} in direction {dir}");

        rb.AddForce(dir * force * (Grounded ? 100 : 20));

        Destroy(Instantiate(jumpParticle, playerHead.transform.position + (Vector3.down * 0.5f), Quaternion.identity), 1f);
    }

    public void ResetTo(Vector3 pos)
    {
        rb.MovePosition(pos);
        rb.SetRotation(0);
    }
}
