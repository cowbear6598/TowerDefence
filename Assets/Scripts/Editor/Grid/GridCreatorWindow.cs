using Grid.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.Grid
{
	[CustomEditor(typeof(GridCreator))]
	public class GridCreatorWindow : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var gridCreator = (GridCreator)target;

			if (GUILayout.Button("建立網格"))
			{
				gridCreator.CreateGrid();
			}

			if (GUILayout.Button("清除網格"))
			{
				while (gridCreator.transform.childCount > 0)
				{
					DestroyImmediate(gridCreator.transform.GetChild(0).gameObject);
				}
			}
		}
	}
}