using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShooter : MonoBehaviour
{
    [Header("총알 세팅")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private Transform yawRoot;
    [SerializeField] private Transform pitchRoot;
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private float mouseSensitivity = 2.0f;

    [Header("탄약 세팅")]
    [SerializeField] private int maxAmmo = 30;       // 한 탄창 용량
    [SerializeField] private int currentAmmo;        // 현재 탄창 잔탄
    [SerializeField] private int totalClips = 3;     // 예비 탄창 개수

    [Header("게임 관리")]
    [SerializeField] public ScoreManager scoreManager;

    float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentAmmo = maxAmmo; // 시작은 풀탄
    }

    void Update()
    {
        // 마우스 회전
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (yawRoot) yawRoot.Rotate(Vector3.up, mx, Space.World);
        pitch = Mathf.Clamp(pitch - my, -85f, 85f);
        if (pitchRoot) pitchRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // 사격
        if (Input.GetMouseButtonDown(0))
            Shoot();

        // 재장전
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    void Shoot()
    {
        if (cam == null || bulletPrefab == null || gunMuzzle == null)
            return;

        if (currentAmmo <= 0)
        {
            Debug.Log("탄약 부족! 재장전 필요");
            return;
        }

        // 총알 생성
        Vector3 dir = cam.transform.forward;
        Vector3 spawnPos = gunMuzzle.position;
        GameObject go = Instantiate(bulletPrefab, spawnPos, Quaternion.LookRotation(dir));
        if (go.TryGetComponent(out Bullet b))
        {
            b.scoreManager = scoreManager;
            b.Shot(dir, bulletSpeed);
        }

        currentAmmo--;
        Debug.Log($"발사! 남은 탄약: {currentAmmo}/{maxAmmo}, 예비 탄창: {totalClips}");

        if (scoreManager != null)
            scoreManager.ConsumeAmmo(currentAmmo, totalClips);
        else
            Debug.LogWarning("ScoreManager 미할당!");
    }

    void Reload()
    {
        if (totalClips > 0 && currentAmmo < maxAmmo)
        {
            totalClips--;      // 예비 탄창 바로 차감
            currentAmmo = maxAmmo;  // 탄창 채움
            Debug.Log($"재장전 완료! 현재 탄약: {currentAmmo}/{maxAmmo}, 예비 탄창: {totalClips}");

            // UI 갱신
            scoreManager?.ConsumeAmmo(currentAmmo, totalClips);
        }
        else
        {
            Debug.Log("재장전 불가 (예비 탄창 없음)");
        }
    }

    // 아이템에서 탄약 충전 가능
    public void AddAmmo(int clips)
    {
        totalClips += clips;
        Debug.Log($"예비 탄창 추가! 현재 예비 탄창: {totalClips}");
    }
}
