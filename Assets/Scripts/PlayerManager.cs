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
        [Header("Textboxes")]
        public TMP_Text moneyText;
        public TMP_Text roundNumber;

        [Header("Actual Stats")]
        public static int currentMoney;
        public static int currentRound;
        // Start is called before the first frame update
        void Awake()
        {
            currentRound = 0;
            currentMoney = 200;
        }

        // Update is called once per frame
        void Update()
        {
            moneyText.text = ("Money: " + currentMoney);
            roundNumber.text = ("Round: " + currentRound);
        }

        public static void GameOver()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}