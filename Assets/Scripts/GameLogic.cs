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
            private set
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
            private set
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

        #region Methods
        /// <summary>
        /// Sets a new Target for the Tag
        /// </summary>
        /// <param name="newTarget">New target to set</param>
        public void SetTarget(TagController newTarget)
        {
            if (this.Target != newTarget)
            {
                this.Target = newTarget;
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

        private void Start()
        {
            //Find closest target to tag
            TagController temp = this.Targets[0];
            float distance = (GridUtils.ProjectPosition(this.Tag.Position, temp.Position) - this.Tag.Position).magnitude;
            for (int i = 1; i < this.Targets.Length; i++)
            {
                TagController t = this.Targets[i];
                if ((GridUtils.ProjectPosition(this.Tag.Position, t.Position) - this.Tag.Position).magnitude < distance)
                {
                    temp = t;
                }
            }
            //Set target
            this.Target = temp;
        }
        #endregion
    }
}
