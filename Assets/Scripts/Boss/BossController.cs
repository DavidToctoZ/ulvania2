using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gm;
    private Animator animator;

    public float rate = 1f;


    int health = 7;
    [SerializeField]
    private int maxHealth = 7;

    public Slider healthSlider;

    void Start()
    {
        animator = GetComponent<Animator>();
        positions.Add(new Vector3(46.25f, 9.4f));
        positions.Add(new Vector3(51.4f, 12.2f));
        positions.Add(new Vector3(31.22f, 12.2f));
        positions.Add(new Vector3(33.13f, 2.33f));
        positions.Add(new Vector3(40.75f, 7.7f));

        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0f;
        healthSlider.value = maxHealth;

    }

    private void FixedUpdate()
    {
        Debug.Log(health);
    }


    public void Hurt(int damage)
    {
        health -= damage;
        healthSlider.value -= damage;
        if (health <= 0)
        {
            Die();
            
        }
    }


    public void Die()
    {
        StartCoroutine("destroyBoss");
        
     

    }

    IEnumerator destroyBoss()
    {
        animator.SetTrigger("destroy");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    List<Vector3> positions = new List<Vector3>();


    int r;
    int cont = 0;
    // Update is called once per frame
    void Update()
    {
       
        
        
        if (Math.Floor(Time.time) % 5 == 0)
        {
            ataqueHoz();
            StartCoroutine("attackHoz");
            cont++;
            if(cont == 1)
            {
                transform.position = positions[getR()];
            }
        }
        else
        {
            cont = 0;
        }

    }

    public int getR()
    {
        return Random.Range(0, 5);
    }
    IEnumerator attackHoz()
    {
        
        yield return new WaitForSeconds(2f);
        isAtaqueHoz = false;
    }

    public void moveTo()
    {
        transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        ataqueHoz();
    }
    public bool getAtaqueHozBool()
    {
        return isAtaqueHoz;
    }

    public bool isAtaqueHoz = false;
    void ataqueHoz()
    {
        isAtaqueHoz = true;
        animator.SetTrigger("attack");
    }
   
}
