using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{

    public GameObject[] characterModels;
    public GameObject[] characterImages;

    // Start is called before the first frame update
    void Start()
    {
        if (CBSceneManager.CBcharacter == 0)
        {
            characterModels[0].SetActive(true);
            characterModels[1].SetActive(false);
            characterModels[2].SetActive(false);

            characterImages[0].SetActive(true);
            characterImages[1].SetActive(false);
            characterImages[2].SetActive(false);


        } else if (CBSceneManager.CBcharacter == 1)
        {
            characterModels[0].SetActive(false);
            characterModels[1].SetActive(true);
            characterModels[2].SetActive(false);

            characterImages[0].SetActive(false);
            characterImages[1].SetActive(true);
            characterImages[2].SetActive(false);

        } else if (CBSceneManager.CBcharacter == 2)
        {
            characterModels[0].SetActive(false);
            characterModels[1].SetActive(false);
            characterModels[2].SetActive(true);

            characterImages[0].SetActive(false);
            characterImages[1].SetActive(false);
            characterImages[2].SetActive(true);
        }
         
    }
}
