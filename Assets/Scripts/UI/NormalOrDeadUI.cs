using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalOrDeadUI : MonoBehaviour
{
    public GameObject player;
    public GameObject normalUI;
    public GameObject deadUI;

    private bool isPlayerDead;

    private void Start()
    {
        normalUI.SetActive(true);
        deadUI.SetActive(false);
    }

    private void Update()
    {
        isPlayerDead = player.GetComponent<PlayerHealth>().isDead;

        if (isPlayerDead)
        {
            normalUI.SetActive(false);
            deadUI.SetActive(true);
        }
    }
}
