using UnityEngine;
using UnityEngine.UI;

namespace COMP476A1.UI
{
    /// <summary>
    /// Pause Menu UI Handler
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private Text title;
        [SerializeField]
        private Button resumeButton;
        #endregion

        #region Methods
        /// <summary>
        /// Show the pause menu
        /// </summary>
        public void Show(bool showing)
        {
            GameLogic.IsPaused = showing;
            this.gameObject.SetActive(showing);
        }

        /// <summary>
        /// Game over actions
        /// </summary>
        public void GameOver()
        {
            Show(true);
            this.title.text = "Game Over!";
            this.resumeButton.interactable = false;
        }

        /// <summary>
        /// Resume button UI event
        /// </summary>
        public void Resume() => Show(false);

        /// <summary>
        /// Restart button UI event
        /// </summary>
        public void Restart()
        {
            GameLogic.IsPaused = false;
            GameLogic.LoadScene(GameScenes.WORLD);
        }

        /// <summary>
        /// Menu button UI event
        /// </summary>
        public void Menu()
        {
            GameLogic.IsPaused = false;
            GameLogic.LoadScene(GameScenes.MENU);
        }

        /// <summary>
        /// Quit button UI event
        /// </summary>
        public void Quit() => GameLogic.Quit();
        #endregion
    }
}
