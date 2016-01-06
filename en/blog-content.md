---
layout : default
title : Andrey Akinshin's blog / Content
permalink: /en/blog/content/index.html
---
@model Pretzel.Logic.Templating.Context.PageContext

<h2>Content</h2>
<div>
@foreach(var category in Model.Site.Categories.OrderByDescending(c => c.Posts.Count()))
{
    var posts = category.Posts.Distinct().ToList();
    if (posts.Count() > 0)
    {
        <h4 id="@category.Name"><a href="#@category.Name">#</a>@category.Name.Replace("dotnet", ".NET").Replace("dev", "Development")</h4>
        <ul>
        @foreach(var post in posts)
        {
            <li><a href='@post.Url.Replace("index.html", "")'>@post.Title</a></li>
        }
        </ul>
    }
}
</div>
<hr />
<p style="font-size:150%"><a href="/ru/blog/content/">More posts in Russian</a></p>