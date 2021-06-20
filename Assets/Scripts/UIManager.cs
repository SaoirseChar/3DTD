using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense.Turret
{
    public class UIManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PauseButton()
        {
            Time.timeScale = 0;
        }

        public void ContinueButton()
        {
            Time.timeScale = 1;
        }

        public void ReloadSceneButton()
        {
            Time.timeScale = 1;
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
        public void MainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        public void QuitButton()
        {
            Time.timeScale = 1;

            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}