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
                    this.tagged.State = TagState.WANDER;
                }

                this.tagged = value;
                this.tagged.State = TagState.TAG;
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
                    if (this.target && !this.target.IsFrozen)
                    {
                        this.target.State = TagState.WANDER;
                    }

                    this.target = value;
                    this.target.State = TagState.TARGET;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Setup the grid and place the players randomly around
        /// </summary>
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
                player.Rotation = (Random.value - 0.5f) * 360f;
            }

            this.Tag = this.Players[Random.Range(0, this.playersCount)];
            this.Targets = this.Players.Where(p => p != this.Tag).ToArray();

            //Find closest target to tag
            SetClosestTarget();
        }

        /// <summary>
        /// Set the new target as the closest to the current tag
        /// </summary>
        public void SetClosestTarget()
        {
            //Find valid targets
            TagController[] valid = this.Targets.Where(t => !t.IsFrozen).ToArray();

            //If none, the tag has won
            if (valid.Length == 0)
            {
                //TODO: Win condition
                Time.timeScale = 0f;
                return;
            }

            //Find closest valid target
            TagController newTarget = valid[0];
            float distance = GridUtils.GetShortestDirection(this.Tag.Position, newTarget.Position).magnitude;
            for (int i = 1; i < valid.Length; i++)
            {
                TagController t = valid[i];
                if (GridUtils.GetShortestDirection(this.Tag.Position, t.Position).magnitude < distance)
                {
                    newTarget = t;
                }
            }

            //Set target
            this.Target = newTarget;
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
