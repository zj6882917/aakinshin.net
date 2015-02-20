---
layout : ru-page
title : Блог Андрея Акиньшина
permalink: /ru/blog/index.html
---
@model Pretzel.Logic.Templating.Context.PageContext

<div class="posts">
@for (var i = 0; i < Model.Site.Posts.Count; i++)
{
    var post = Model.Site.Posts[i];
    var excerpt = (string)post.Bag["excerpt"];
    if (post.Categories.First() == "ru")
    {
        <div class="idea">
            <h2><a href="@post.Url">@post.Title</a></h2>
            <div class="postdate">
              <b>Дата:</b> @post.Date.ToString("dd MMMM, yyyy", new System.Globalization.CultureInfo("ru-RU")). <b>Теги:</b>
                @foreach(var tag in post.Tags)
                {
                    <span>[</span><span>@tag</span><span>]&nbsp;</span>
                }
            </div>
            @Raw(excerpt)
            <a href='@post.Url.Replace("index.html", "")'>Читать дальше</a>
            <br />
        </div>
    }
}
</div>