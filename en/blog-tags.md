---
layout : default
title : Andrey Akinshin's blog / Tags
permalink: /en/blog/tags/index.html
---
@model Pretzel.Logic.Templating.Context.PageContext

<h2>Tags</h2>
<div>
@foreach(var tag in Model.Site.Tags.OrderByDescending(c => c.Posts.Count()))
{
    var posts = tag.Posts.Distinct().ToList();
    if (posts.Count() > 0)
    {
        <h3 id="@tag.Name">@tag.Name</h3>
        <ul>
        @foreach(var post in posts)
        {
            <li><a href='@post.Url.Replace("index.html", "")'>@post.Title</a></li>
        }
        </ul>
    }
}
</div>