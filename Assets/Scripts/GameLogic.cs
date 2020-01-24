using System.Linq;
using UnityEngine;

namespace COMP476A1
{
    /// <summary>
    /// Singleton GameLogic object
    /// </summary>
    public class GameLogic : MonoBehaviour
    {
        #region Instance
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static GameLogic Instance { get; private set; }
        #endregion

        #region Properties
        /// <summary>
        /// Array of all targets for the Tag
        /// </summary>
        public TagController[] Targets { get; private set; }

        private TagController tagged;
        /// <summary>
        /// Tagged character
        /// </summary>
        public TagController Tag
        {
            get => this.tagged;
            set
            {
                if (this.tagged)
                {
                    this.tagged.IsTag = false;
                }

                this.tagged = value;
                this.tagged.IsTag = true;
            }
        }

        private TagController target;
        /// <summary>
        /// Currently targetted character
        /// </summary>
        public TagController Target
        {
            get => this.target;
            set
            {
                if (this.target)
                {
                    this.target.IsTarget = false;
                }

                this.target = value;
                this.target.IsTarget = true;
            }
        }
        #endregion

        #region Functions
        private void Awake()
        {
            //Make sure only one Singleton instance
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            //Get all Characters
            TagController[] players = FindObjectsOfType<TagController>();
            this.Tag = players[Random.Range(0, players.Length)];
            this.Targets = players.Where(p => p != this.Tag).ToArray();
        }
        #endregion
    }
}
