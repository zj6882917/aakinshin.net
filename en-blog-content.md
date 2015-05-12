---
layout : en-page
title : Andrey Akinshin's blog / Content
permalink: /en/blog/content/index.html
---
@model Pretzel.Logic.Templating.Context.PageContext

<div class="posts">
@foreach(var category in Model.Site.Categories.OrderByDescending(c => c.Posts.Count()))
{
    if (category.Name != "ru" && category.Name != "en")
    {
        var posts = category.Posts.Where(p => p.Categories.First() == "en").Distinct().ToList();
        if (posts.Count() > 0)
        {
            <h2>@category.Name.Replace("dotnet", ".NET").Replace("dev", "Development")</h2>
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
<hr />
<p style="font-size:200%"><a href="/ru/blog/content/">More posts in Russian</a></p>