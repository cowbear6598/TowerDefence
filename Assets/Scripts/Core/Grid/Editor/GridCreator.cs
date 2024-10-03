using Grid.Views;
using UnityEngine;

namespace Grid.Editor
{
	public class GridCreator : MonoBehaviour
	{
		public int   _gridWidth  = 20;
		public int   _gridHeight = 20;
		public float _cellSize   = 1;

		public GridView _gridViewPrefab;

		#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			for (var x = 0; x < _gridWidth; x++)
			{
				for (var z = 0; z < _gridHeight; z++)
				{
					var position = transform.position + new Vector3(x * _cellSize, 0, z * _cellSize) - CenterOffset;

					Gizmos.DrawWireCube(position, new Vector3(_cellSize, 0, _cellSize));
				}
			}
		}

		public void CreateGrid()
		{
			while (transform.childCount > 0)
			{
				DestroyImmediate(transform.GetChild(0).gameObject);
			}

			for (var x = 0; x < _gridWidth; x++)
			{
				for (var z = 0; z < _gridHeight; z++)
				{
					var position = transform.position + new Vector3(x * _cellSize, 0, z * _cellSize) - CenterOffset;

					var cell = Instantiate(_gridViewPrefab, position, Quaternion.Euler(90, 0, 0), transform);
					cell.name = $"Cell_{x}_{z}";
				}
			}
		}

		private Vector3 CenterOffset => new(
			(_gridWidth - 1) * _cellSize * 0.5f,
			0,
			(_gridHeight - 1) * _cellSize * 0.5f
		);

		#endif
	}
}