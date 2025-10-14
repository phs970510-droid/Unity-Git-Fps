using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShooter : MonoBehaviour
{
    [Header("�Ѿ� ����")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private Transform yawRoot;
    [SerializeField] private Transform pitchRoot;
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private float mouseSensitivity = 2.0f;

    [Header("ź�� ����")]
    [SerializeField] private int maxAmmo = 30;       // �� źâ �뷮
    [SerializeField] private int currentAmmo;        // ���� źâ ��ź
    [SerializeField] private int totalClips = 3;     // ���� źâ ����

    [Header("���� ����")]
    [SerializeField] public ScoreManager scoreManager;

    float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentAmmo = maxAmmo; // ������ Ǯź
    }

    void Update()
    {
        // ���콺 ȸ��
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (yawRoot) yawRoot.Rotate(Vector3.up, mx, Space.World);
        pitch = Mathf.Clamp(pitch - my, -85f, 85f);
        if (pitchRoot) pitchRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // ���
        if (Input.GetMouseButtonDown(0))
            Shoot();

        // ������
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    void Shoot()
    {
        if (cam == null || bulletPrefab == null || gunMuzzle == null)
            return;

        if (currentAmmo <= 0)
        {
            Debug.Log("ź�� ����! ������ �ʿ�");
            return;
        }

        // �Ѿ� ����
        Vector3 dir = cam.transform.forward;
        Vector3 spawnPos = gunMuzzle.position;
        GameObject go = Instantiate(bulletPrefab, spawnPos, Quaternion.LookRotation(dir));
        if (go.TryGetComponent(out Bullet b))
        {
            b.scoreManager = scoreManager;
            b.Shot(dir, bulletSpeed);
        }

        currentAmmo--;
        Debug.Log($"�߻�! ���� ź��: {currentAmmo}/{maxAmmo}, ���� źâ: {totalClips}");

        if (scoreManager != null)
            scoreManager.ConsumeAmmo(currentAmmo, totalClips);
        else
            Debug.LogWarning("ScoreManager ���Ҵ�!");
    }

    void Reload()
    {
        if (totalClips > 0 && currentAmmo < maxAmmo)
        {
            totalClips--;      // ���� źâ �ٷ� ����
            currentAmmo = maxAmmo;  // źâ ä��
            Debug.Log($"������ �Ϸ�! ���� ź��: {currentAmmo}/{maxAmmo}, ���� źâ: {totalClips}");

            // UI ����
            scoreManager?.ConsumeAmmo(currentAmmo, totalClips);
        }
        else
        {
            Debug.Log("������ �Ұ� (���� źâ ����)");
        }
    }

    // �����ۿ��� ź�� ���� ����
    public void AddAmmo(int clips)
    {
        totalClips += clips;
        Debug.Log($"���� źâ �߰�! ���� ���� źâ: {totalClips}");
    }
}
