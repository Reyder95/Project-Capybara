using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public float variance = 0.5f;

    public Vector2 initPos;
    public Vector2 nextPos;

    public void Start()
    {
        initPos = transform.position;
        nextPos = new Vector2(initPos.x, initPos.y - variance);
    }

    public void Update()
    {
        transform.position = Vector2.Lerp(transform.position, nextPos, 0.5f * Time.deltaTime);
        Debug.Log(Vector2.Distance(transform.position, nextPos));
        if (Vector2.Distance(transform.position, nextPos) < 0.2)
        {
            if (nextPos.y < initPos.y)
                nextPos = new Vector2(initPos.x, initPos.y + variance);
            else
                nextPos = new Vector2(initPos.x, initPos.y - variance);
        }
            
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Game.Instance.playerInfo.abilityList.Add(Game.Instance.abilityDictionary["Blue Wave"]);
            Destroy(this.gameObject);
        }
    }
}
