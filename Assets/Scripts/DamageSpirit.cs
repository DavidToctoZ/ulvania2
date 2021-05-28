using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpirit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gm;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Hero")
        {

            gm.damageMinionAttack();
            Debug.Log("minion");
            transform.position = new Vector3(-100f, -100f);

        }
    }
}
