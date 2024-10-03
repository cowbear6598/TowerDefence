using System;
using Grid.Domain;
using Grid.Views;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Grid.Handlers
{
	public class GridVisualizeHandler : IInitializable, ITickable, IDisposable
	{
		[Inject] private readonly Settings  _settings;
		[Inject] private readonly GridBoard _gridBoard;

		private const string GRID_LAYER_NAME = "Grid";

		private GridView[,] _gridViews;

		private NativeArray<GridVisualData> _currentGridVisualData;
		private NativeArray<GridVisualData> _targetGridVisualData;

		private int _highlightIndex = -1;

		public void Initialize()
		{
			CreateGridView();
		}

		public void Tick()
		{
			UpdateGridVisualJob();

			// TODO: Refactor this to a separate class
			var cam = Camera.main;

			var ray = cam.ScreenPointToRay(Input.mousePosition);

			Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

			if (!Physics.Raycast(ray, out var hit, 100f, LayerMask.GetMask(GRID_LAYER_NAME)))
			{
				if (_highlightIndex == -1)
					return;

				_targetGridVisualData[_highlightIndex] = new GridVisualData(0.2f, 0);
				_highlightIndex                        = -1;

				return;
			}

			var x = Mathf.FloorToInt(hit.point.x / _gridBoard.CellSize);
			var z = Mathf.FloorToInt(hit.point.z / _gridBoard.CellSize);

			var newHighlightIndex = x + z * _gridBoard.Width;

			if (_highlightIndex == newHighlightIndex)
				return;

			if (_highlightIndex != -1)
				_targetGridVisualData[_highlightIndex] = new GridVisualData(0.2f, 0);

			_highlightIndex = newHighlightIndex;

			_targetGridVisualData[_highlightIndex] = new GridVisualData(1f, 0.5f);

		}

		private void UpdateGridVisualJob()
		{
			var job = new GridVisualUpdateJob
			{
				CurrentGridVisualData = _currentGridVisualData,
				TargetGridVisualData  = _targetGridVisualData,
				LerpSpeed             = 10f,
				DeltaTime             = Time.deltaTime,
			};

			var handle = job.Schedule(_currentGridVisualData.Length, 64);
			handle.Complete();

			for (var i = 0; i < _currentGridVisualData.Length; i++)
			{
				var data = _currentGridVisualData[i];

				_gridViews[i % _gridBoard.Width, i / _gridBoard.Width].UpdateVisualData(data);
			}
		}

		private void CreateGridView()
		{
			var cellParent = new GameObject("CellGroup").transform;

			var width    = _gridBoard.Width;
			var height   = _gridBoard.Height;
			var cellSize = _gridBoard.CellSize;

			_currentGridVisualData = new NativeArray<GridVisualData>(width * height, Allocator.Persistent);
			_targetGridVisualData  = new NativeArray<GridVisualData>(width * height, Allocator.Persistent);

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

					var index = x + z * width;

					_currentGridVisualData[index] = new GridVisualData(0.2f, 0);
					_targetGridVisualData[index]  = new GridVisualData(0.2f, 0);
				}
			}
		}

		public void Dispose() { }

		[BurstCompile]
		private struct GridVisualUpdateJob : IJobParallelFor
		{
			[ReadOnly] public NativeArray<GridVisualData> TargetGridVisualData;
			[ReadOnly] public float                       LerpSpeed;
			[ReadOnly] public float                       DeltaTime;

			public NativeArray<GridVisualData> CurrentGridVisualData;

			public void Execute(int index)
			{
				var currentData = CurrentGridVisualData[index];
				var targetData  = TargetGridVisualData[index];

				var alpha = math.lerp(currentData.Alpha, targetData.Alpha, LerpSpeed   * DeltaTime);
				var y     = math.lerp(currentData.Height, targetData.Height, LerpSpeed * DeltaTime);

				CurrentGridVisualData[index] = new GridVisualData(alpha, y);
			}
		}

		[Serializable]
		public class Settings
		{
			public GridView GridViewPrefab;
			public float    LerpSpeed = 10;

			public float originalAlpha  = 0.2f;
			public float highlightAlpha = 1f;

			public float originalHeight  = 0;
			public float highlightHeight = 0.25f;
		}
	}
}