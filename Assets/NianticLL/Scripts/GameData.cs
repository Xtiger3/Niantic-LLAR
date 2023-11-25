using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Inst;

    private List<string> MakiHex = new List<string> {"#F3EAFF", "#E1C8FF", "#BAA1EE", "#AF89FF"};
    private List<string> OBHex = new List<string> {"#DFEEFF", "#B9D1FF", "#95CCFF", "#4596F6"};
    private List<string> ZeroHex = new List<string> {"#D1D1D1", "#919191", "#545454", "#3A3A3A"};

    private List<Color> MakiPalette = new List<Color>();
    private List<Color> OBPalette = new List<Color>();
    private List<Color> ZeroPalette = new List<Color>();
    
    public Dictionary<string, List<Color>> NPCColor = new Dictionary<string, List<Color>>();

    private void Awake() {
        Color c;
        for (int i = 0; i < MakiHex.Count; i++) {
            ColorUtility.TryParseHtmlString(MakiHex[i], out c);
            MakiPalette.Add(c);
            ColorUtility.TryParseHtmlString(OBHex[i], out c);
            OBPalette.Add(c);
            ColorUtility.TryParseHtmlString(ZeroHex[i], out c);
            ZeroPalette.Add(c);
        }

        NPCColor.Add("Maki", MakiPalette);
        NPCColor.Add("Zero", ZeroPalette);
        NPCColor.Add("OB", OBPalette);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Inst == null) {
            Inst = this;
        } else {
            Destroy(this);
        }
        Debug.Log(NPCColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
