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

        #region Constants
        /// <summary>
        /// Limit at which players can be placed on the grid
        /// </summary>
        public const float PLACEMENT_LIMIT = GridUtils.GRID_SIZE * 0.9f;
        #endregion

        #region Fields
        [SerializeField]
        private int playersCount = 5;
        [SerializeField]
        private TagController playerPrefab;
        [SerializeField]
        private Transform world;
        #endregion

        #region Properties
        /// <summary>
        /// Array of all the players
        /// </summary>
        public TagController[] Players { get; private set; }

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
            set
            {
                if (this.target != value)
                {
                    if (this.target)
                    {
                        this.target.IsTarget = false;
                    }

                    this.target = value;
                    this.target.IsTarget = true;
                }
            }
        }
        #endregion

        #region Methods
        private void PlacePlayers()
        {
            //Setup player array
            this.Players = new TagController[this.playersCount];

            //Create the right amount of players
            for (int i = 0; i < this.playersCount; i++)
            {
                TagController player = Instantiate(this.playerPrefab, this.world);
                this.Players[i] = player;
                player.SetupComponents();
                player.Position = new Vector2(Random.value - 0.5f, Random.value - 0.5f) * PLACEMENT_LIMIT;
            }

            this.Tag = this.Players[Random.Range(0, this.playersCount)];
            this.Targets = this.Players.Where(p => p != this.Tag).ToArray();

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

        #region Functions
        private void Awake()
        {
            //Make sure only one Singleton instance
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            //Initialize instance
            Instance = this;
            DontDestroyOnLoad(this);
            Random.InitState(new System.Random().Next());

            //Setup players
            PlacePlayers();
        }
        #endregion
    }
}
