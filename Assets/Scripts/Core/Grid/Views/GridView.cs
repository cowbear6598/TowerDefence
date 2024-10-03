using UnityEngine;

namespace Grid.Views
{
	public class GridView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _render;

		[SerializeField] private Color _originColor;
		[SerializeField] private Color _errorColor;
		[SerializeField] private Color _highlightColor;

		public void Reset()     => _render.color = _originColor;
		public void Error()     => _render.color = _errorColor;
		public void Highlight() => _render.color = _highlightColor;
	}
}