using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("�κ��丮 UI")]
    [SerializeField] private GameObject inventoryCanvas; // �κ��丮 ��ü UI
    private bool isOpen = false;

    void Start()
    {
        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(false); // ���� �� ���� ����
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;

        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(isOpen);

        // Ŀ�� ���� (���� ���콺 Ȱ��ȭ)
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }
}
