using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHoz : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gm;
    public BossController boss;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Hero" && boss.getAtaqueHozBool())
        {

            gm.damageHozAttack();
            
        }
    }
}
