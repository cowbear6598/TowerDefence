using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Grid.Domain
{
	public class GridBoard : IInitializable
	{
		[Inject] private readonly Settings _settings;

		private Grid[,] _grid;

		public void Initialize()
		{
			_grid = new Grid[_settings.Width, _settings.Height];

			for (var x = 0; x < _settings.Width; x++)
			{
				for (var z = 0; z < _settings.Height; z++)
				{
					_grid[x, z] = new Grid(new Vector2Int(x, z));
				}
			}
		}

		public int   Width    => _settings.Width;
		public int   Height   => _settings.Height;
		public float CellSize => _settings.CellSize;

		[Serializable]
		public class Settings
		{
			public int   Width;
			public int   Height;
			public float CellSize;
		}
	}
}