---
layout : ru-page
title : Блог Андрея Акиньшина / Содержание
permalink: /ru/blog/content/index.html
---
@model Pretzel.Logic.Templating.Context.PageContext

<div class="posts">
@foreach(var category in Model.Site.Categories.OrderByDescending(c => c.Posts.Count()))
{
    if (category.Name != "ru" && category.Name != "en")
    {
        var posts = category.Posts.Where(p => p.Categories.First() == "ru").ToList();
        if (posts.Count() > 0)
        {
            <h2>@category.Name.Replace("dotnet", ".NET").Replace("dev", "Разработка").Replace("notes", "Заметки").Replace("education", "Образование").Replace("r", "R")</h2>
            <ul>
            @foreach(var post in posts)
            {
                <li><a href='@post.Url.Replace("index.html", "")'>@post.Title</a></li>
            }
            </ul>
        }
    }
}
</div>