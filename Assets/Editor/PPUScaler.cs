using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel.Design;
using System.Diagnostics;

public class PPUScaler : EditorWindow
{
    [MenuItem("Tools/Scale Sprites After PPU Change")]
    static void ScaleSpritesAfterPPUChange()
    {
        float oldPPU = 48f;
        float newPPU = 32f;
        float scaleMultiplier = oldPPU / newPPU;

        foreach (GameObject obj in Selection.gameObjects)
        {
            obj.transform.localScale *= scaleMultiplier;
        }

        UnityEngine.Debug.Log("Scaled selected GameObjects by" + scaleMultiplier);
    }
}
