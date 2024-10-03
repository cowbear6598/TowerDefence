using Grid.Views;
using UnityEngine;
using VContainer.Unity;

namespace Grid.Handlers
{
	public class GridVisualizeHandler : ITickable
	{
		private const string GRID_LAYER_NAME = "Grid";

		private GridView _currentGridView;

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

			var newGridView = hit.collider.gameObject.GetComponent<GridView>();

			if (_currentGridView == newGridView)
				return;

			_currentGridView?.Reset();

			_currentGridView = newGridView;
			_currentGridView.Highlight();
		}
	}
}