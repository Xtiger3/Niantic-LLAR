using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FCButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Button FCbutton;
    private VocabDisplayManager vocabDisplayRef;
    private FCManager FCManagerRef;
    private string prefabName;
    void Start()
    {
        


        prefabName = gameObject.name;
        vocabDisplayRef = FindObjectOfType<VocabDisplayManager>();
        FCManagerRef = FindObjectOfType<FCManager>();

        if (vocabDisplayRef == null)
        {
            Debug.LogError("SceneScript not found in the scene.");
        }
        FCbutton.onClick.AddListener(() => CallFunctionInSceneScript());
        
    }



    public void CallFunctionInSceneScript()
    {
        if (vocabDisplayRef != null)
        {
            vocabDisplayRef.ClearEverything();
        }
        else
        {
            Debug.LogError("vocabDisplay reference is not set.");
        }

        if (FCManagerRef != null)
        {
            FCManagerRef.InitializeFC(prefabName);
        }
        else
        {
            Debug.LogError("FC script reference is not set.");
        }
    }
}
