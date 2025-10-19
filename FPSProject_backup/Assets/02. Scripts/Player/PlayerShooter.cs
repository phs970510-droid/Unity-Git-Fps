using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("��� / �� ����")]
    [SerializeField] private float fireRate = 0.1f;     // ���� ����
    [SerializeField] private float zoomFOV = 20f;       // ������ ���� (3�� ��)
    [SerializeField] private float normalFOV = 60f;     // �⺻ �þ�
    [SerializeField] private float zoomSpeed = 10f;     // �� ��ȯ �ε巯��
    [SerializeField] private float zoomSensitivityMultiplier = 0.5f; // �� �� ���� ����

    [Header("�ѱ� ����")]
    [SerializeField] private Transform gunRoot;  // �� ��Ʈ
    [SerializeField] private Vector3 normalPos = new Vector3(0.4f, -0.3f, 0.5f); // ��� ��ġ
    [SerializeField] private Vector3 zoomPos = new Vector3(0f, -0.1f, 0.8f);    // �� �� �߾� ��ġ
    [SerializeField] private float alignSpeed = 10f; // �̵� �ӵ�

    [Header("UI")]
    [SerializeField] private Image crosshair;     // ������ �̹���
    [SerializeField] private Image scopeOverlay;  // ������ �������� �̹��� (�� �� ǥ��)

    [Header("ź�� ����")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int totalClips = 3;

    [Header("���� ����")]
    [SerializeField] public ScoreManager scoreManager;

    public bool isInventoryOpen = false;

    private float pitch;
    private float nextFireTime;
    private bool isZooming;
    private bool isAutoFire = true; // true: ���� / false: �ܹ�
    private float originalSensitivity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentAmmo = maxAmmo;
        originalSensitivity = mouseSensitivity;

        if (cam != null)
            cam.fieldOfView = normalFOV;

        if (crosshair != null)
            crosshair.enabled = false;

        if (scopeOverlay != null)
            scopeOverlay.enabled = false;
    }

    void Update()
    {
        if (InputLockManager.Blocked)
            return;

        HandleLook();
        HandleFire();
        HandleReload();
        HandleZoom();
        HandleFireModeSwitch();
        HandleGunAlignment(); // �ѱ� ���� ó��
    }

    // ���콺 ȸ��
    void HandleLook()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (yawRoot) yawRoot.Rotate(Vector3.up, mx, Space.World);
        pitch = Mathf.Clamp(pitch - my, -85f, 85f);
        if (pitchRoot) pitchRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    // �߻� ó�� (����/�ܹ�)
    void HandleFire()
    {
        if (isAutoFire)
        {
            // ���� ���: ��Ŭ�� ����
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
        }
        else
        {
            // �ܹ� ���: ��Ŭ�� ������ ����
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    // ������
    void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    // ����/�ܹ� ��ȯ (BŰ)
    void HandleFireModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isAutoFire = !isAutoFire;
            string mode = isAutoFire ? "����" : "�ܹ�";
            Debug.Log($"��� ��� ��ȯ: {mode}");
        }
    }

    void HandleZoom()
    {
        if (cam == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            isZooming = true;
            mouseSensitivity = originalSensitivity * zoomSensitivityMultiplier;

            // �� �� ������ �������̸� ǥ��
            if (scopeOverlay != null) scopeOverlay.enabled = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isZooming = false;
            mouseSensitivity = originalSensitivity;

            // �� ���� �� �������� ��Ȱ��ȭ
            if (scopeOverlay != null) scopeOverlay.enabled = false;
        }

        float targetFOV = isZooming ? zoomFOV : normalFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    // �� �� �ѱ� ��ġ �߾� ����
    void HandleGunAlignment()
    {
        if (gunRoot == null) return;

        Vector3 targetPos = isZooming ? zoomPos : normalPos;
        gunRoot.localPosition = Vector3.Lerp(gunRoot.localPosition, targetPos, Time.deltaTime * alignSpeed);
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

        Vector3 dir = cam.transform.forward;
        GameObject go = Instantiate(bulletPrefab, gunMuzzle.position, Quaternion.LookRotation(dir));

        if (go.TryGetComponent(out Bullet b))
        {
            b.scoreManager = scoreManager;
            b.Shot(dir, bulletSpeed);
        }

        currentAmmo--;
        scoreManager?.ConsumeAmmo(currentAmmo, totalClips);
        Debug.Log($"�߻�! ���� ź��: {currentAmmo}/{maxAmmo}, ���� źâ: {totalClips}");
    }

    void Reload()
    {
        if (totalClips > 0 && currentAmmo < maxAmmo)
        {
            totalClips--;
            currentAmmo = maxAmmo;
            scoreManager?.ConsumeAmmo(currentAmmo, totalClips);
            Debug.Log($"������ �Ϸ�! ���� ź��: {currentAmmo}/{maxAmmo}, ���� źâ: {totalClips}");
        }
        else
        {
            Debug.Log("������ �Ұ� (���� źâ ����)");
        }
    }

    public void AddAmmo(int clips)
    {
        totalClips += clips;
        scoreManager?.ConsumeAmmo(currentAmmo, totalClips);
        Debug.Log($"���� źâ �߰�! ���� ���� źâ: {totalClips}");
    }
}