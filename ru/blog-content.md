---
layout : default
title : Блог Андрея Акиньшина / Содержание
permalink: /ru/blog/content/index.html
---
@model Pretzel.Logic.Templating.Context.PageContext

<h2>Содержание</h2>
<div>
<p><a href="/en/blog/content/">Последние посты доступны только в английской версии блога</a></p>
<hr />
@foreach(var category in Model.Site.Categories.OrderByDescending(c => c.Posts.Count()))
{
    var posts = category.Posts.Distinct().ToList();
    if (posts.Count() > 0)
    {
        <h3 id="@category.Name">@category.Name.Replace("dotnet", ".NET").Replace("dev", "Разработка").Replace("notes", "Заметки").Replace("education", "Образование").Replace("r", "R")</h3>
        <ul>
        @foreach(var post in posts)
        {
            <li><a href='@post.Url.Replace("index.html", "")'>@post.Title</a> <i>(@post.Date.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("ru-RU")))</i></li>
        }
        </ul>
    }
}
</div>
<hr />
<p>Подписаться: <a href="/ru/rss.xml">RSS</a> <a href="/ru/atom.xml">Atom</a></p>