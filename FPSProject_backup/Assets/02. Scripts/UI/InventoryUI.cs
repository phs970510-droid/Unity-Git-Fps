using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("인벤토리 UI")]
    [SerializeField] private GameObject inventoryCanvas; // 인벤토리 전체 UI
    private bool isOpen = false;

    void Start()
    {
        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(false); // 시작 시 닫힘 상태
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

        // 커서 제어 (열면 마우스 활성화)
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }
}
