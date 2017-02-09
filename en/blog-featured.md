---
layout : default
title : Andrey Akinshin's blog / Featured posts
permalink: /en/blog/featured/index.html
---
@model Pretzel.Logic.Templating.Context.PageContext

<h2>Featured posts</h2>
<div>
@{
    var featuredTags = new string[] { "Internals", "PerformanceExercise", "Rider" };
    var featuredTagsTitles = new string[] { ".NET Internals", "Performance Exercises", "Rider stories" };
}@for (int i = 0; i < featuredTags.Length; i++)
{
    var tagName = featuredTags[i];
    var tagTitle = featuredTagsTitles[i]; 
    var tag = Model.Site.Tags.First(c => c.Name == tagName);
    var posts = tag.Posts.Distinct().ToList();
    if (posts.Count() > 0)
    {
        <h3 id="@tag.Name">@tagTitle</h3>
        <ul>
        @foreach(var post in posts)
        {
            <li><a href='@post.Url.Replace("index.html", "")'>@post.Title</a> <i>(@post.Date.ToString("MMMM dd, yyyy", new System.Globalization.CultureInfo("en-US")))</i></li>
        }
        </ul>
    }
}
</div>
<hr />
<p style="font-size:150%"><a href="/ru/blog/content/">More posts in Russian</a></p>
<p>Subscribe: <a href="/en/rss.xml">RSS</a> <a href="/en/atom.xml">Atom</a></p>