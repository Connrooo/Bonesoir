using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] Image[] heartSprites;
    [SerializeField] Sprite brokenHeartSprite;
    public int health = 5;

    public void hurt()
    {
        heartSprites[--health].sprite = brokenHeartSprite;
        if (health<=0)
        {
            Debug.Log("dead");
        }
    }
}
