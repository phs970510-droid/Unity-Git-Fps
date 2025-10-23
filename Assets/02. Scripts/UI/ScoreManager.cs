using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [Header("ź�� ����")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI clipsText; // ���� źâ �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI restartText;

    public int Score { get; private set; }

    // ���� Ammo�� Clips�� PlayerShooter���� ������
    [HideInInspector] public int CurrentAmmo;
    [HideInInspector] public int TotalClips;

    void Awake()
    {
        ResetSession();
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("FPSField");
        }
    }

    public void ResetSession()
    {
        Score = 0;
        CurrentAmmo = maxAmmo;
        TotalClips = 3; // �⺻ ���� źâ ����
    }

    // PlayerShooter���� �Ѿ� �Һ� �� ȣ��
    public void ConsumeAmmo(int currentAmmo, int totalClips)
    {
        CurrentAmmo = currentAmmo;
        TotalClips = totalClips;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText) scoreText.text = "Score: " + Score;
        if (ammoText) ammoText.text = $"Ammo: {CurrentAmmo}/{maxAmmo}";
        if (clipsText) clipsText.text = $"Clips: {TotalClips}";
    }
}
