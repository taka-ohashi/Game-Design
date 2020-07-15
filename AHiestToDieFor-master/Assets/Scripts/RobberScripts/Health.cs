using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    private GlobalEventManager gem;
    private Animator animator;
    public int maxHealth;
    private int health;

    private void Awake()
    {
        List<MonoBehaviour> deps = new List<MonoBehaviour>
        {
            (gem = FindObjectOfType(typeof(GlobalEventManager)) as GlobalEventManager),
        };
        if (deps.Contains(null))
        {
            throw new Exception("Could not find dependency");
        }
        animator = GetComponent<Animator>();
        this.health = maxHealth;
    }

    void Start()
    {
        gem.StartListening("Shot", TakeDamage);
    }

    public void OnDestroy()
    {
        gem.StopListening("Shot", TakeDamage);
    }

    public void TakeDamage(GameObject target, List<object> parameters)
    {
        if (target != gameObject)
        {
            return;
        }

        health -= 1;
        if (health == 0)
        {
            animator.SetBool("isDead", true);
            gameObject.tag = "Corpse";
            gem.TriggerEvent("Death", gameObject);
            // triggers event in a lot of places,
            // GameManager, SelecedManager, CorpseLeaving, MoneyBag, Movement, Unlocking
            // basically all the places that need to be stopped or removed from a list when someone dies
            // and the money being dropped ofc
        }
    }
}
