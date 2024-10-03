namespace Grid.Domain
{
	public struct GridVisualData
	{
		public readonly float Alpha;
		public readonly float Height;

		public GridVisualData(float alpha, float height)
		{
			Alpha  = alpha;
			Height = height;
		}
	}
}