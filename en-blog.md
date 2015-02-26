---
layout : en-page
title : Andrey Akinshin's blog
permalink: /en/blog/index.html
---
@model Pretzel.Logic.Templating.Context.PageContext

<div class="posts">
@for (var i = 0; i < Model.Site.Posts.Count; i++)
{
    var post = Model.Site.Posts[i];
    var excerpt = (string)post.Bag["excerpt"];
    if (post.Categories.First() == "en")
    {
        <div class="idea">
            <h2><a href='@post.Url.Replace("index.html", "")'>@post.Title</a></h2>
            @Raw(excerpt)
            <a href='@post.Url.Replace("index.html", "")'>Read more</a><br /><br />
            <span class="postdate">
              <b>Date:</b> @post.Date.ToString("MMMM dd, yyyy", new System.Globalization.CultureInfo("en-US")). <b>Tags:</b>
                @foreach(var tag in post.Tags)
                {
                    <span>[</span><span>@tag</span><span>]</span>
                }
            </span>
            <hr />
        </div>
    }
}
</div>
<hr />
<p style="font-size:200%"><a href="/ru/blog/">More posts in Russian</a></p>