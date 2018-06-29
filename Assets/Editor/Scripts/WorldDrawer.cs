using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class WorldDrawer : ScriptableWizard
{

    [MenuItem("My Tools/ Open World Drawer")]
    static void SelectWorldMode()
    {
        ScriptableWizard.DisplayWizard<WorldDrawer>("World Drawer", "Open World Drawer");
    }

    private void OnWizardCreate()
    {
        Camera camera = Camera.current;
        
    }
}
