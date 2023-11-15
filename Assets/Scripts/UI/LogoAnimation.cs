using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoAnimation : MonoBehaviour
{
    public Image image;
    public List<Sprite> sprites;

    private void Start()
    {
        StartCoroutine(Coroutine());
        image.sprite = sprites[sprites.Count - 1];
    }

    IEnumerator Coroutine()
    {
        for (int i = 0; i < sprites.Count - 1; i++)
        {
            image.sprite = sprites[i];
            yield return new WaitForSeconds(0.04f);
        }
    }
}
