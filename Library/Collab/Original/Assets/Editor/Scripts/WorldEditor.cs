using UnityEngine;
using UnityEditor;

public class WorldEditor : EditorWindow
{
    enum DisplayMode {GameView, DrawView, CodeView};
    DisplayMode currentDisplayMode;

    Texture2D worldMap;
    Color color;

    [MenuItem("Window/World Editor")]
	static void Init()
    {
        WorldEditor worldEditor = (WorldEditor)EditorWindow.GetWindow(typeof(WorldEditor));
        worldEditor.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        currentDisplayMode = (DisplayMode)EditorGUILayout.EnumPopup("View Mode", currentDisplayMode);

        switch (currentDisplayMode)
        {
            case DisplayMode.GameView:

                break;

            case DisplayMode.DrawView:
                color = EditorGUILayout.ColorField("Pencil Color", color);
                break;

            case DisplayMode.CodeView:

                break;
        }


        #region Buttons
        if (Selection.gameObjects.Length != 0)
        {
            if (Selection.gameObjects[0].tag == "WorldCanvas")
            {
                if (GUILayout.Button("Edit Selected Canvas"))
                {
                    currentDisplayMode = DisplayMode.DrawView;
                    Vector3 position = SceneView.lastActiveSceneView.pivot;
                    position = new Vector3(Selection.gameObjects[0].transform.position.x, Selection.gameObjects[0].transform.position.y, Selection.gameObjects[0].transform.position.z);
                    SceneView.lastActiveSceneView.pivot = position;
                    SceneView.lastActiveSceneView.Repaint();
                }
            }
        }
        else
        {
            currentDisplayMode = DisplayMode.GameView;
            if (GUILayout.Button("Spawn Canvas"))
            {
                GameObject WorldCanvas = GameObject.FindGameObjectWithTag("WorldCanvas");
                
            }
        }
        #endregion

        #region Compile All Canvases
        
        #endregion
    }
}
