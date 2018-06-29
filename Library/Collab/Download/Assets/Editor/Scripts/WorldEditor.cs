using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WorldEditor : EditorWindow
{
    enum DisplayMode {GameView, DrawView, CodeView};
    DisplayMode currentDisplayMode;

    List<GameObject> canvasArray;
    Texture2D worldMap;
    Texture2D emptyTexture;
    Color color;
    float brushSize;
    GameObject currentCanvas;

    [MenuItem("Window/World Editor")]
	static void Init()
    {
        WorldEditor worldEditor = (WorldEditor)EditorWindow.GetWindow(typeof(WorldEditor));
        worldEditor.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        GUILayout.Label(currentDisplayMode.ToString());
        //SetToolMode((DisplayMode)EditorGUILayout.EnumPopup("View Mode", currentDisplayMode));

        if (GUILayout.Button("Spawn Canvas"))
        {
            Selection.activeObject = SceneView.currentDrawingSceneView;

            Vector3 position = SceneView.lastActiveSceneView.pivot;

            //Vector3 position = Camera.current.transform.position;

            emptyTexture = new Texture2D(512, 512);
            for (int y = 0; y < 512; y++)
            {
                for (int x = 0; x < 512; x++)
                {
                    emptyTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
            emptyTexture.Apply();

            Texture2D canvasTexture = emptyTexture;
            Sprite canvas = Sprite.Create(canvasTexture, new Rect(0, 0, 512, 512), new Vector2(0.5f, 0.5f), 128f);
            GameObject spriteObject = new GameObject();
            spriteObject.transform.position = new Vector3(position.x, position.y, 0);
            spriteObject.name = "worldSprite";
            spriteObject.tag = "WorldCanvas";
            spriteObject.AddComponent<SpriteRenderer>().sprite = canvas;
            spriteObject.AddComponent<WorldEditorCanvas>();
            SceneView.lastActiveSceneView.Repaint();
        }
           
        if(currentDisplayMode == DisplayMode.DrawView)
        {
            color = EditorGUILayout.ColorField("Pencil Color", color);
            brushSize = EditorGUILayout.Slider(brushSize, 1, 100);



            if (GUILayout.Button("Finish Drawing"))
            {
                SetToolMode(DisplayMode.GameView);
                Sprite drawSprite = currentCanvas.GetComponent<SpriteRenderer>().sprite;
                for (int y = 0; y < 512; y++)
                {
                    for (int x = 0; x < 512; x++)
                    {
                        if (drawSprite.texture.GetPixel(x, y) == new Color(1, 1, 1, 1))
                        {
                            drawSprite.texture.SetPixel(x, y, new Color(0,0,0,0));
                        }
                    }
                }
                drawSprite.texture.Apply();
                currentCanvas.GetComponent<SpriteRenderer>().sprite = drawSprite;
            }
        }
    }

    void OnFocus()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        WorldEditorCanvas.Deleted -= this.RenewCanvasList;
        WorldEditorCanvas.Deleted += this.RenewCanvasList;
        WorldEditorCanvas.Created -= this.RenewCanvasList;
        WorldEditorCanvas.Created += this.RenewCanvasList;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        WorldEditorCanvas.Deleted -= this.RenewCanvasList;
        WorldEditorCanvas.Created -= this.RenewCanvasList;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (currentDisplayMode == DisplayMode.GameView)
        {
            foreach (GameObject obj in canvasArray)
            {
                if (Handles.Button(obj.transform.position, Quaternion.identity, 5.12f / 2f, 5.12f / 2f, Handles.RectangleHandleCap /*Handles.DrawSolidRectangleWithOutline*/))
                {
                    Debug.Log("The button was pressed!");
                    Debug.Log("Entering Edit-Mode!");
                    EnterEditMode(obj);
                }
            }
        }
    }

    void EnterEditMode(GameObject canvasObject)
    {
        SetToolMode(DisplayMode.DrawView);
        currentCanvas = canvasObject;
        Sprite drawSprite = canvasObject.GetComponent<SpriteRenderer>().sprite;
        for (int y = 0; y < 512; y++)
        {
            for (int x = 0; x < 512; x++)
            {
                if (drawSprite.texture.GetPixel(x, y) == new Color(0, 0, 0, 0))
                {
                    drawSprite.texture.SetPixel(x, y, Color.white);
                }
            }
        }
        drawSprite.texture.Apply();
        canvasObject.GetComponent<SpriteRenderer>().sprite = drawSprite;

        Vector3 position = SceneView.lastActiveSceneView.pivot;
        position = new Vector3(canvasObject.transform.position.x, canvasObject.transform.position.y, canvasObject.transform.position.z);
        SceneView.lastActiveSceneView.pivot = position;
        SceneView.lastActiveSceneView.Repaint();
    }

    void SetToolMode(DisplayMode mode)
    {
        currentDisplayMode = mode;
        switch (currentDisplayMode)
        {
            case DisplayMode.GameView:
                
                break;

            case DisplayMode.DrawView:
     
                break;

            case DisplayMode.CodeView:

                break;
        }
    }

    void RenewCanvasList()
    {
        canvasArray.Clear();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("WorldCanvas"))
        {
            canvasArray.Add(obj);
        }
    }
}
