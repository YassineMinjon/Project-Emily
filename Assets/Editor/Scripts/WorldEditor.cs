using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WorldEditor : EditorWindow
{
    enum DisplayMode {GameView, DrawView, CodeView};
    DisplayMode currentDisplayMode;
    enum BrushShape { Round, Square, Custom};
    BrushShape currentBrushShape;

    List<GameObject> canvasArray;
    Texture2D worldMap;
    Texture2D emptyTexture;
    Vector2Int canvasSize;
    Color color;
    float brushSize;
    GameObject currentCanvas;
    float pxDensity = 1000;

    [MenuItem("Window/World Editor")]
	static void Init()
    {
        WorldEditor worldEditor = (WorldEditor)EditorWindow.GetWindow(typeof(WorldEditor));
        worldEditor.Show();
    }

    void Update()
    {
        if(currentDisplayMode == DisplayMode.DrawView)
        {
            SceneView.RepaintAll();
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        GUILayout.Label(currentDisplayMode.ToString());
        //SetToolMode((DisplayMode)EditorGUILayout.EnumPopup("View Mode", currentDisplayMode));

        if (currentDisplayMode == DisplayMode.GameView)
        {
            canvasSize = EditorGUILayout.Vector2IntField("Canvas Scale", canvasSize);
            if (GUILayout.Button("Spawn Canvas"))
            {
                SpawnWorldCanvas();
            }
        }
           
        if(currentDisplayMode == DisplayMode.DrawView)
        {
            color = EditorGUILayout.ColorField("Pencil Color", color);
            currentBrushShape = (BrushShape)EditorGUILayout.EnumPopup("Brush Shape", currentBrushShape);
            brushSize = EditorGUILayout.Slider(brushSize, 1f, 100f);


            if (GUILayout.Button("Finish Drawing"))
            {
                SetToolMode(DisplayMode.GameView);
                Sprite drawSprite = currentCanvas.GetComponent<SpriteRenderer>().sprite;
                for (int y = 0; y < currentCanvas.GetComponent<WorldEditorCanvas>().size.y; y++)
                {
                    for (int x = 0; x < currentCanvas.GetComponent<WorldEditorCanvas>().size.x; x++)
                    {
                        if (drawSprite.texture.GetPixel(x, y) == new Color(1, 1, 1, 1))
                        {
                            drawSprite.texture.SetPixel(x, y, new Color(0,0,0,0));
                        }
                    }
                }
                drawSprite.texture.Apply();
                currentCanvas.GetComponent<SpriteRenderer>().sprite = drawSprite;
                currentCanvas.GetComponent<WorldEditorCanvas>().SetColliderType(typeof(PolygonCollider2D));
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
        if(canvasArray.Count==0)
        {
            return;
        }
        if (currentDisplayMode == DisplayMode.GameView)
        {
            foreach (GameObject obj in canvasArray)
            {
                if (Handles.Button(obj.transform.position, Quaternion.identity, obj.GetComponent<WorldEditorCanvas>().size.x/pxDensity, obj.GetComponent<WorldEditorCanvas>().size.x/pxDensity, Handles.RectangleHandleCap))
                {
                    Debug.Log("The button was pressed!");
                    Debug.Log("Entering Edit-Mode!");
                    EnterEditMode(obj);
                }
            }
        }

        else if(currentDisplayMode == DisplayMode.DrawView)
        {
            RaycastHit hit;
            Event e = Event.current;
            Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.gameObject == currentCanvas)
                {
                    Handles.color = color;
                    Handles.DrawSolidDisc(hit.point, hit.normal, (1/pxDensity * brushSize)/2f);
                    DrawOnCanvas(e, hit, hit.transform.gameObject);
                }
            }
        }
        SceneView.RepaintAll();
    }

    void SpawnWorldCanvas()
    {
        Selection.activeObject = SceneView.currentDrawingSceneView;

        Vector3 position = SceneView.lastActiveSceneView.pivot;

        emptyTexture = new Texture2D(canvasSize.x, canvasSize.y);
        for (int y = 0; y < canvasSize.y; y++)
        {
            for (int x = 0; x < canvasSize.x; x++)
            {
                emptyTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        emptyTexture.Apply();

        Texture2D canvasTexture = emptyTexture;
        Sprite canvas = Sprite.Create(canvasTexture, new Rect(0, 0, canvasSize.x, canvasSize.y), new Vector2(0.5f, 0.5f), pxDensity);
        GameObject spriteObject = new GameObject();
        spriteObject.transform.position = new Vector3(position.x, position.y, 0);
        spriteObject.name = "worldSprite";
        spriteObject.tag = "WorldCanvas";
        spriteObject.AddComponent<SpriteRenderer>().sprite = canvas;
        spriteObject.AddComponent<WorldEditorCanvas>().size = canvasSize;
        spriteObject.AddComponent<PolygonCollider2D>();
        SceneView.lastActiveSceneView.Repaint();
    }

    void EnterEditMode(GameObject canvasObject)
    {
        SetToolMode(DisplayMode.DrawView);
        currentCanvas = canvasObject;
        Sprite drawSprite = canvasObject.GetComponent<SpriteRenderer>().sprite;
        for (int y = 0; y < currentCanvas.GetComponent<WorldEditorCanvas>().size.y; y++)
        {
            for (int x = 0; x < currentCanvas.GetComponent<WorldEditorCanvas>().size.x; x++)
            {
                if (drawSprite.texture.GetPixel(x, y) == new Color(0, 0, 0, 0))
                {
                    drawSprite.texture.SetPixel(x, y, Color.white);
                }
            }
        }
        drawSprite.texture.Apply();
        canvasObject.GetComponent<SpriteRenderer>().sprite = drawSprite;
        canvasObject.GetComponent<WorldEditorCanvas>().SetColliderType(typeof(BoxCollider));
        

        Vector3 position = SceneView.lastActiveSceneView.pivot;
        position = new Vector3(canvasObject.transform.position.x, canvasObject.transform.position.y, canvasObject.transform.position.z);
        SceneView.lastActiveSceneView.pivot = position;
        SceneView.lastActiveSceneView.Repaint();
    }

    void DrawOnCanvas(Event e, RaycastHit hit, GameObject canvasObject)
    {
        if((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) )
        {
            Event.current.Use();
            Texture2D texture = canvasObject.GetComponent<SpriteRenderer>().sprite.texture;
            float width = canvasObject.GetComponent<SpriteRenderer>().sprite.bounds.max.x - canvasObject.GetComponent<SpriteRenderer>().sprite.bounds.min.x;
            float heigth = canvasObject.GetComponent<SpriteRenderer>().sprite.bounds.max.y - canvasObject.GetComponent<SpriteRenderer>().sprite.bounds.min.y;
            float pxWdt = texture.width / width;
            float pxHgt = texture.height / heigth;
            float x = (hit.point.x - canvasObject.transform.position.x) * pxWdt + texture.width / 2;
            float y = (hit.point.y - canvasObject.transform.position.y) * pxHgt + texture.height / 2;

            List<Vector2Int> kernel = BrushKernel(x,y);

            for (int i = 0; i < kernel.Count; i++)
            {
                canvasObject.GetComponent<SpriteRenderer>().sprite.texture.SetPixel(kernel[i].x, kernel[i].y, color);
            }
            canvasObject.GetComponent<SpriteRenderer>().sprite.texture.Apply();
            SceneView.RepaintAll();
        }
    }

    List<Vector2Int> BrushKernel(float hitx, float hity)
    {
        List<Vector2Int> kernel = new List<Vector2Int>();

        for (int y = (int)hity + (int)(-1f * (brushSize / 2f)); y < (int)hity + (int)(brushSize / 2f); y++)
        {
            for (int x = (int)hitx + (int)(-1f * (brushSize / 2f)); x < (int)hitx + (int)(brushSize / 2f); x++)
            {
                if (currentBrushShape == BrushShape.Round)
                {
                    if (Vector2Int.Distance(new Vector2Int((int)hitx, (int)hity), new Vector2Int(x, y)) <= (brushSize / 2f))
                    {
                        kernel.Add(new Vector2Int(x, y));
                    }
                }
                else if( currentBrushShape == BrushShape.Square)
                {
                    kernel.Add(new Vector2Int(x, y));
                }
            }
        }
        
        return kernel;
    }

    void SetToolMode(DisplayMode mode)
    {
        currentDisplayMode = mode;
        switch (currentDisplayMode)
        {
            case DisplayMode.GameView:
                Tools.current = Tool.View;
                HandleUtility.Repaint();
                break;

            case DisplayMode.DrawView:
                Tools.current = Tool.None;
                Tools.hidden = true;
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
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
