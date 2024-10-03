using Grid.Domain;
using UnityEngine;

namespace Grid.Views
{
	public class GridView : MonoBehaviour
	{
		[SerializeField] private Transform      _spriteTransform;
		[SerializeField] private SpriteRenderer _render;

		public void UpdateVisualData(GridVisualData data)
		{
			_render.color                  = new Color(_render.color.r, _render.color.g, _render.color.b, data.Alpha);
			_spriteTransform.localPosition = new Vector3(_spriteTransform.localPosition.x, data.Height, _spriteTransform.localPosition.z);
		}
	}
}