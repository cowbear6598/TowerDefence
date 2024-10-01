using UnityEngine;

namespace Grid.Domain
{
	public class Cell
	{
		public readonly Vector2Int Position;

		private GameObject OccupyingObject;

		public Cell(Vector2Int position)
		{
			Position = position;
		}

		public void SetOccupyingObject(GameObject occupyingObject) => OccupyingObject = occupyingObject;
		public bool IsOccupied()                                   => OccupyingObject != null;
	}
}