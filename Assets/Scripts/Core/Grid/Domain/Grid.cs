using UnityEngine;

namespace Grid.Domain
{
	public class Grid
	{
		public readonly Vector2Int Position;

		private GameObject OccupyingObject;

		public Grid(Vector2Int position)
		{
			Position = position;
		}

		public void SetOccupyingObject(GameObject occupyingObject) => OccupyingObject = occupyingObject;
		public bool IsOccupied()                                   => OccupyingObject != null;
	}
}