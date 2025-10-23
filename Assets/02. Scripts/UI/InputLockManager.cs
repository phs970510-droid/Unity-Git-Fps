using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �Է� ��� �Ŵ���.
/// - Acquire("Inventory") : �Է� ��� ON
/// - Release("Inventory") : �Է� ��� ����
/// - InputLockManager.Blocked : ��� �� ���� (true�� FPS �Է� ��� ����)
/// Ŀ�� ǥ��/���� ���⼭ �ϰ� ����.
/// </summary>
public class InputLockManager : MonoBehaviour
{
    public static InputLockManager Instance { get; private set; }

    // Ȱ��ȭ�� ��� Ű ����(�ߺ� ��� ����)
    private readonly HashSet<string> _locks = new HashSet<string>();

    /// <summary>����� 1�� �̻��̸� true</summary>
    public static bool Blocked => Instance != null && Instance._locks.Count > 0;

    // ��� ���°� �ٲ� �� �˸��� �ʿ��ϸ� �̺�Ʈ �����ؼ� ���� ��.
    public static System.Action<bool> OnLockStateChanged; // �Ű�����: Blocked

    [Header("Ŀ��/�� �⺻ ��å")]
    [Tooltip("����� ���� �� Ŀ���� ǥ������ ����(�Ϲ������� true)")]
    [SerializeField] private bool showCursorWhenBlocked = true;

    [Tooltip("����� ���� �� Ŀ���� ����� ����(�Ϲ������� true)")]
    [SerializeField] private bool lockCursorWhenFree = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �ߺ� ����
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ApplyCursorPolicy();
    }

    /// <summary>��� ȹ�� (��: "Inventory", "Pause", "Settings")</summary>
    public static void Acquire(string key)
    {
        if (string.IsNullOrEmpty(key)) return;
        if (Instance == null) CreateBootstrap();

        bool before = Blocked;
        Instance._locks.Add(key);
        if (Blocked != before)
        {
            Instance.ApplyCursorPolicy();
            OnLockStateChanged?.Invoke(Blocked);
        }
    }

    /// <summary>��� ����</summary>
    public static void Release(string key)
    {
        if (Instance == null || string.IsNullOrEmpty(key)) return;
        bool before = Blocked;
        Instance._locks.Remove(key);
        if (Blocked != before)
        {
            Instance.ApplyCursorPolicy();
            OnLockStateChanged?.Invoke(Blocked);
        }
    }

    /// <summary>��� ��� ��� ���� (�����/����)</summary>
    public static void ReleaseAll()
    {
        if (Instance == null) return;
        bool before = Blocked;
        Instance._locks.Clear();
        if (Blocked != before)
        {
            Instance.ApplyCursorPolicy();
            OnLockStateChanged?.Invoke(Blocked);
        }
    }

    private static void CreateBootstrap()
    {
        var go = new GameObject("[InputLockManager]");
        go.AddComponent<InputLockManager>();
    }

    private void ApplyCursorPolicy()
    {
        if (Blocked)
        {
            Cursor.visible = showCursorWhenBlocked;
            Cursor.lockState = showCursorWhenBlocked ? CursorLockMode.None : CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = !lockCursorWhenFree ? true : false;
            Cursor.lockState = lockCursorWhenFree ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}