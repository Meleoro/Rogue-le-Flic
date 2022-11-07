using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrailController : MonoBehaviour
{
    public GameObject DashTrail;
    public float delay = 1.0f;
    float delta = 0;

    DashChara player;
    SpriteRenderer spriteRenderer;
    public float destroyTime = 0.1f;
    public Color color;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delta > 0) { delta -= Time.deltaTime; }
        else { delta = delay; createDash(); }
    }

    void createDash()
    {
        GameObject dashObj = Instantiate(DashTrailPrefab, transform.position, transform.rotation);
        ghostObj.transform.localScale = player.transform.localScale;
        destroy(dashObj, destroyTime);

        spriteRenderer = dashObj.GetComponent<spriteRenderer>();
        spriteRenderer.sprite = player.SpriteRenderer.sprite;
        spriteRenderer.color = color;
        if (material != null) spriteRenderer.material = material;
    }
}
