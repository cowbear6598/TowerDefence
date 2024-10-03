using System;
using Grid.Domain;
using Grid.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Grid.Handlers
{
	public class GridVisualizeHandler : IInitializable, ITickable
	{
		[Inject] private readonly Settings  _settings;
		[Inject] private readonly GridBoard _gridBoard;

		private const string GRID_LAYER_NAME = "Grid";

		private GridView[,] _gridViews;

		private GridView _currentGridView;

		public void Initialize()
		{
			CreateGridView();
		}

		public void Tick()
		{
			// TODO: Refactor this to a separate class
			var cam = Camera.main;

			var ray = cam.ScreenPointToRay(Input.mousePosition);

			Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

			if (!Physics.Raycast(ray, out var hit, 100f, LayerMask.GetMask(GRID_LAYER_NAME)))
			{
				if (_currentGridView == null)
					return;

				_currentGridView.Reset();
				_currentGridView = null;

				return;
			}

			var x = Mathf.FloorToInt(hit.point.x / _gridBoard.CellSize);
			var z = Mathf.FloorToInt(hit.point.z / _gridBoard.CellSize);

			var newGridView = _gridViews[x, z];

			if (_currentGridView == newGridView)
				return;

			_currentGridView?.Reset();

			_currentGridView = newGridView;
			_currentGridView.Highlight();
		}

		private void CreateGridView()
		{
			var cellParent = new GameObject("CellGroup").transform;

			var width    = _gridBoard.Width;
			var height   = _gridBoard.Height;
			var cellSize = _gridBoard.CellSize;

			_gridViews = new GridView[width, height];

			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < height; z++)
				{
					var gridView = Object.Instantiate(_settings.GridViewPrefab, cellParent);
					gridView.gameObject.name = $"Cell_{x}_{z}";

					gridView.transform.localPosition = new Vector3(
						x * cellSize + cellSize / 2.0f,
						0,
						z * cellSize + cellSize / 2.0f
					);

					gridView.transform.localScale = Vector3.one * cellSize;

					_gridViews[x, z] = gridView;
				}
			}
		}

		[Serializable]
		public class Settings
		{
			public GridView GridViewPrefab;
		}
	}
}