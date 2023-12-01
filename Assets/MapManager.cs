using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public static MapManager Inst;
    public string NPCName;
    public Vector3 NPCPos;

    // Start is called before the first frame update
    void Start()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
