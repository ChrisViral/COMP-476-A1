using UnityEngine;

namespace COMP476A1
{
    public static class GridUtils
    {
        #region Constants
        /// <summary>
        /// Grid size
        /// </summary>
        public const float GRID_SIZE = 10f;

        /// <summary>
        /// Grid wrap limit
        /// </summary>
        public const float WRAP_LIMIT = GRID_SIZE / 2f;
        #endregion

        #region Static methods
        /// <summary>
        /// Wraps a vector around the grid
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Vector2 WrapPosition(Vector2 pos)
        {
            //Check X axis wrap
            if (Mathf.Abs(pos.x) >= WRAP_LIMIT)
            {
                pos = new Vector2(pos.x - (Mathf.Sign(pos.x) * GRID_SIZE), pos.y);
            }
            //Check Y axis wrap
            if (Mathf.Abs(pos.y) >= WRAP_LIMIT)
            {
                pos = new Vector2(pos.x, pos.y - (Mathf.Sign(pos.y) * GRID_SIZE));
            }
            //Return wrapped position
            return pos;
        }
        #endregion
    }
}