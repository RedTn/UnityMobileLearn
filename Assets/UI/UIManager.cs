using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public UISprite[] lifeSprites;

    public UILabel scoreLabel;

    public static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("UI Manager");
                    _instance = container.AddComponent<UIManager>();
                }
            }

            return _instance;
        }
    }

	// Use this for initialization
	void Start () {
        for (int i = 0; i < lifeSprites.Length; i++)
            lifeSprites[i].enabled = true;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateLifeSprites(int numLives)
    {
        for (int i = numLives; i < lifeSprites.Length; i++)
        {
            lifeSprites[i].enabled = false;
        }
    }

    public void UpdateScore(int newScore)
    {
        scoreLabel.text = newScore.ToString();
    }
}
