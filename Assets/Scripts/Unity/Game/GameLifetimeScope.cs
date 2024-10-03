using System;
using Grid.Handlers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity.Game
{
	public class GameLifetimeScope : LifetimeScope
	{
		[SerializeField] private GridSetting gridSetting;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterInstance(gridSetting.GridSettings);
			builder.RegisterInstance(gridSetting.GridVisualizeSettings);

			builder.Register<Grid.Domain.GridBoard>(Lifetime.Singleton);
			builder.Register<GridVisualizeHandler>(Lifetime.Singleton)
			       .AsImplementedInterfaces()
			       .AsSelf();
		}

		[Serializable]
		public class GridSetting
		{
			public Grid.Domain.GridBoard.Settings     GridSettings;
			public GridVisualizeHandler.Settings GridVisualizeSettings;
		}
	}
}