using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.GridLayoutGroup;

public class Bullet : MonoBehaviour
{
    public enum BulletOwner { Player, Enemy }
    public BulletOwner owner;

    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private int baseDamage = 40;
    private Rigidbody bulletRigid;
    [SerializeField] public ScoreManager scoreManager;

    private void Awake()
    {
        bulletRigid = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.0f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == BulletOwner.Player && other.CompareTag("EnemyHead"))
        {
            EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(baseDamage * 3);
                scoreManager?.AddScore(150);
            }
            Destroy(gameObject);
        }
        else if (owner == BulletOwner.Player && other.CompareTag("EnemyBody"))
        {
            EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(baseDamage);
                scoreManager?.AddScore(100);
            }
            Destroy(gameObject);
        }
        else if (owner == BulletOwner.Enemy && other.CompareTag("PlayerBody"))
        {
            PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(baseDamage);
            }
            Destroy(gameObject);
        }
        else if (owner == BulletOwner.Enemy && other.CompareTag("PlayerHead"))
        {
            PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(baseDamage * 3);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    //�Ѿ��� �߻�� �� ������ ����� �ӵ��� ������ٵ� �����ؼ� ������ ���ư��� ��
    public void Shot(Vector3 dir, float speed)
    {
        moveSpeed = speed;
        //rigid�� �ӵ� ����
        bulletRigid.velocity = dir * moveSpeed;
    }
}
