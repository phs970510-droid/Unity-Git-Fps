using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private Animator anim; // ��� �ִϸ��̼ǿ� (������ ����)
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        currentHealth = 0;

        if (anim != null)
        {
            anim.SetTrigger("Die");
        }
        else
        {
            // �ӽ� ó��: ����� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }
}
