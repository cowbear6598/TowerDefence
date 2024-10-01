using UnityEngine;

namespace Grid.Views
{
	public class GridView : MonoBehaviour
	{
		[SerializeField] private Transform      _spriteTransform;
		[SerializeField] private SpriteRenderer _render;

		private float _targetAlpha = 0.2f;
		private float _targetY     = 0.0f;

		private void Update()
		{
			var lerpSpeed = 10f;

			var alpha = Mathf.MoveTowards(_render.color.a, _targetAlpha, Time.deltaTime              * lerpSpeed);
			var y     = Mathf.MoveTowards(_spriteTransform.localPosition.y, _targetY, Time.deltaTime * lerpSpeed);

			_render.color                  = new Color(_render.color.r, _render.color.g, _render.color.b, alpha);
			_spriteTransform.localPosition = new Vector3(_spriteTransform.localPosition.x, y, _spriteTransform.localPosition.z);
		}

		public void ResetColor()
		{
			_targetAlpha = 0.2f;
			_targetY     = 0.0f;
		}

		public void Highlight()
		{
			_targetAlpha = 1.0f;
			_targetY     = 0.25f;
		}
	}
}