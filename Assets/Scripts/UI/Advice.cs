using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Advice : MonoBehaviour 
{
    //public string[] urls;
    //public GameObject[] panels;
    //public UnityEvent myEvent;

    //public void Start()
    //{
    //}


    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    Debug.Log(eventData);
    //}

    public void OpenUrl(string url)
    {
        if (url.Contains("https://") || url.Contains("http://"))
        {
            Application.OpenURL(url);
            Debug.Log(url);
        }
        Debug.Log("Do Stuff");
    }

}


// https://www.unian.ua/health/worldnews/naykorisnishi-produkti-dlya-zhinochogo-zdorov-ya-ostanni-novini-11197046.html
// https://harchi.info/articles/yaki-produkty-pryskoryuyut-metabolizm
// https://alexus.com.ua/produkti-dlya-zhinochogo-zdorovya-pravila-zdorovogo-xarchuvannya/
// https://vikna.tv/styl-zhyttya/zdorovia-ta-krasa/yak-nabraty-vagu-shvydko-ta-bezpechno-dlya-zdorovya-porady-diyetologa/
// https://life.pravda.com.ua/health/2019/02/10/235516/
// https://santamaria.com.ua/about/blog/yak-pokrashiti-yakist-snu




