using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("�κ��丮 UI")]
    [SerializeField] private GameObject inventoryCanvas; // �κ��丮 ��ü UI
    private bool isOpen;

    void Start()
    {
        if (inventoryCanvas) inventoryCanvas.SetActive(false);
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
        if (inventoryCanvas) inventoryCanvas.SetActive(isOpen);

        if (isOpen)
            InputLockManager.Acquire("Inventory"); // ���
        else
            InputLockManager.Release("Inventory"); // ����
    }
}
