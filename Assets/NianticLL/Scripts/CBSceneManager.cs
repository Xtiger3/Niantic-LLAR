using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CBSceneManager : MonoBehaviour
{
    public static int CBcharacter;
    // 0  is zero
    // 1 is maki
    // 2 is ob

    public void LoadScene(int characterNum)
    {
        CBcharacter = characterNum;
        SceneManager.LoadScene("CharacterProfile");
    }
}