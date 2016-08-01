---
layout : default
title : Andrey Akinshin's blog
permalink: /en/blog/index.html
paginate: 10
paginate_link: "/en/blog/page/:page/index.html"
---
@model Pretzel.Logic.Templating.Context.PageContext

<div class="blog-main">
@foreach (var post in Model.Paginator.Posts)
{
    var excerpt = (string)post.Bag["excerpt"];
    var category = post.Categories.First();
    var categoryTitle = category.Replace("dotnet", ".NET").Replace("dev", "Development");
    var commentsLink = @post.Url.Replace("index.html", "") + "#disqus_thread";
    <div class="blog-post">
        <h2 class="blog-post-title"><a href='@post.Url.Replace("index.html", "")'>@post.Title</a></h2>
        <span class="blog-post-meta">
          <b>Date:</b> @post.Date.ToString("MMMM dd, yyyy", new System.Globalization.CultureInfo("en-US")).
          <b>Category:</b> <a href="/en/blog/content/#@category"><span class="label label-info">@categoryTitle</span></a>
          <b>Tags:</b>
            @foreach(var tag in post.Tags)
            {
                <a href="/en/blog/tags/#@tag"><span class="label label-pill label-info">@tag</span></a>
            }
        </span><br /><br />
        @Raw(excerpt)
        <a href='@post.Url.Replace("index.html", "")'>Read more</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="@commentsLink">Comments</a><br /><br />
        <hr />
    </div>
}
</div>

<nav>
  <ul class="pagination">
    @if (Model.Paginator.PreviousPageUrl != null)
    {
      <li class="page-item">
        <a class="page-link" href='@Model.Paginator.PreviousPageUrl.Replace("index.html", "")' aria-label="Previous">
          <span aria-hidden="true">&laquo;</span>
          <span class="sr-only">Previous</span>
        </a>
      </li>
    }
    @if (Model.Paginator.PreviousPageUrl == null)
    {
      <li class="page-item disabled">
        <a class="page-link" href="#" aria-label="Previous">
          <span aria-hidden="true">&laquo;</span>
          <span class="sr-only">Previous</span>
        </a>
      </li>
    }
    @for (int i = 1; i <= Model.Paginator.TotalPages; i++)
    {
      var link = i == 1 ? "/en/blog/" : "/en/blog/page/" + i.ToString() + "/";
      if (Model.Paginator.Page == i)
      {
        <li class="page-item active">
          <a class="page-link" href="@link">@i <span class="sr-only">(current)</span></a>
        </li>
      }
      if (Model.Paginator.Page != i)
      {
        <li class="page-item"><a class="page-link" href="@link">@i</a></li>
      }
    }
    @if (Model.Paginator.NextPageUrl != null)
    {
      <li class="page-item">
        <a class="page-link" href='@Model.Paginator.NextPageUrl.Replace("index.html", "")' aria-label="Next">
          <span aria-hidden="true">&raquo;</span>
          <span class="sr-only">Next</span>
        </a>
      </li>
    }
    @if (Model.Paginator.NextPageUrl == null)
    {
      <li class="page-item disabled">
        <a class="page-link" href="#" aria-label="Next">
          <span aria-hidden="true">&raquo;</span>
          <span class="sr-only">Next</span>
        </a>
      </li>
    }
  </ul>
</nav>

<hr />
<p style="font-size:150%"><a href="/ru/blog/">More posts in Russian</a></p>
<p>Subscribe: <a href="/en/rss.xml">RSS</a> <a href="/en/atom.xml">Atom</a></p>