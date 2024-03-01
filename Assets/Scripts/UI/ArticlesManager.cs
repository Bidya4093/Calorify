
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArticlesManager : MonoBehaviour 
{
    private VisualElement mainRoot;
    private TemplateContainer articlePage;
    private TemplateContainer adviceTopicsPage;
    private VisualElement adviceTopicPage;
    private List<VisualElement> adviceArticles;
    private List<VisualElement> adviceTopicArticle;
    private VisualElement bottomMenu;
    private Button closeArticleBtn;
    void Start()
    {
        mainRoot = GetComponent<UIDocument>().rootVisualElement;
        bottomMenu = mainRoot.Q<VisualElement>("BottomMenu");
        adviceTopicsPage = mainRoot.Q<TemplateContainer>("AdviceTopicsPage");
        adviceTopicPage = mainRoot.Q<VisualElement>("AdviceTopicPage");
        adviceArticles = adviceTopicsPage.Query<VisualElement>("AdviceArticles").ToList();
        Debug.Log(adviceTopicPage);
        adviceTopicArticle = adviceTopicPage.Query<VisualElement>("AdviceTopicArticles").ToList();
        articlePage = mainRoot.Q<TemplateContainer>("ArticlePage");
        closeArticleBtn = articlePage.Q<Button>("CloseBtn");

        adviceArticles.ForEach(article =>
        {
            article.RegisterCallback<ClickEvent>(OpenArticle);
        });

        adviceTopicArticle.ForEach(article =>
        {
            Debug.Log(article);
            article.RegisterCallback<ClickEvent>(OpenArticle);
        });

        closeArticleBtn.RegisterCallback<ClickEvent>(CloseArticle);
    }

    void OpenArticle(ClickEvent evt)
    {
        adviceTopicPage.style.display = DisplayStyle.None;
        adviceTopicsPage.style.display = DisplayStyle.None;
        articlePage.style.display = DisplayStyle.Flex;
        bottomMenu.style.display = DisplayStyle.None;
    }

    private void CloseArticle(ClickEvent evt)
    {
        adviceTopicsPage.style.display = DisplayStyle.Flex;
        articlePage.style.display = DisplayStyle.None;
        bottomMenu.style.display = DisplayStyle.Flex;
    }
}


// https://www.unian.ua/health/worldnews/naykorisnishi-produkti-dlya-zhinochogo-zdorov-ya-ostanni-novini-11197046.html
// https://harchi.info/articles/yaki-produkty-pryskoryuyut-metabolizm
// https://alexus.com.ua/produkti-dlya-zhinochogo-zdorovya-pravila-zdorovogo-xarchuvannya/
// https://vikna.tv/styl-zhyttya/zdorovia-ta-krasa/yak-nabraty-vagu-shvydko-ta-bezpechno-dlya-zdorovya-porady-diyetologa/
// https://life.pravda.com.ua/health/2019/02/10/235516/
// https://santamaria.com.ua/about/blog/yak-pokrashiti-yakist-snu




