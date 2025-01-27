using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcDialogue : MonoBehaviour
{
    [SerializeField]
    bool StartOpen = true;
    [SerializeField]
    List<GameObject> Dialogues;
    int CurrentIndex = 0;

    bool OverlapsPlayer = false;

    InputAction InteractAction;
    InputSystem_Actions inputSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputSystem = new InputSystem_Actions();
        inputSystem.Enable();

        foreach (GameObject go in Dialogues)
        {
            go.SetActive(false);
        }
        if (StartOpen)
        {
            Dialogues[0].SetActive(true);
        }

        InteractAction = inputSystem.Player.Interact;
    }

    // Update is called once per frame
    void Update()
    {
        if (OverlapsPlayer)
        {
            if (InteractAction.WasPressedThisFrame())
            {
                if (!StartOpen && CurrentIndex == 0 && !Dialogues[0].activeSelf)
                {
                    Dialogues[0].SetActive(true);
                }
                else
                {
                    if (CurrentIndex == (Dialogues.Count - 1))
                    {
                        return;
                    }
                    Dialogues[CurrentIndex].SetActive(false);
                    CurrentIndex++;
                    Dialogues[CurrentIndex].SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OverlapsPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OverlapsPlayer = false;
        }
    }
}
