using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

namespace TowerDefense.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager thisInstance;

        void Awake()
        {
            if (thisInstance != null)
            {
                Debug.LogError("More than one BuildManager in scene!");
                return;
            }
            thisInstance = this;
        }

        public static bool GameIsOver;
        public GameObject gameOverUI;
        public GameObject WinnerUI;

        [Header("Textboxes")]
        public TMP_Text moneyText;
        public TMP_Text roundNumber;
        public TMP_Text roundNumber2;

        [Header("Actual Stats")]
        public static int currentMoney;
        public static int currentRound;
        // Start is called before the first frame update
        void Start()
        {
            GameIsOver = false;
            currentRound = 0;
            currentMoney = 4000;
        }

        // Update is called once per frame
        void Update()
        {
            moneyText.text = ("Money: " + currentMoney);
            roundNumber.text = ("Round: " + currentRound);
            roundNumber2.text = ("Round: " + currentRound);
        }
        public void IncreaseRound()
        {
            currentRound++;
        }
        public void GameOver()
        {
            EndGame();
        }

        void EndGame()
        {
            GameIsOver = true;
            gameOverUI.SetActive(true);
        }

        public void WinGame()
        {
            GameIsOver = true;
            WinnerUI.SetActive(true);
        }
    }
}