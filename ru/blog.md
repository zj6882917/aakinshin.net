---
layout : default
title : Блог Андрея Акиньшина
permalink: /ru/blog/index.html
paginate: 10
paginate_link: "/ru/blog/page/:page/index.html"
---
@model Pretzel.Logic.Templating.Context.PageContext

<div class="blog-main">
@foreach (var post in Model.Paginator.Posts)
{
    var excerpt = (string)post.Bag["excerpt"];
    var category = post.Categories.First();
    var categoryTitle = category.Replace("dotnet", ".NET").Replace("dev", "Разработка").Replace("notes", "Заметки").Replace("education", "Образование").Replace("r", "R");
    <div class="blog-post">
        <h2 class="blog-post-title"><a href='@post.Url.Replace("index.html", "")'>@post.Title</a></h2>
        @Raw(excerpt)
        <a href='@post.Url.Replace("index.html", "")'>Читать дальше</a><br /><br />
        <span class="blog-post-meta">
          <b>Дата:</b> @post.Date.ToString("dd MMMM, yyyy", new System.Globalization.CultureInfo("ru-RU")).
          <b>Категория:</b> <a href="/ru/blog/content/#@category">@categoryTitle</a>.
          <b>Теги:</b>
            @foreach(var tag in post.Tags)
            {
                <a href="/ru/blog/tags/#@tag"><span>[</span><span>@tag</span><span>]</span></a>
            }
        </span>
        <hr />
    </div>
}
</div>

<nav>
  <ul class="pagination">
    @if (Model.Paginator.PreviousPageUrl != null)
    {
      <li class="page-item">
        <a class="page-link" href='@Model.Paginator.PreviousPageUrl.Replace("index.html", "")' aria-label="Назад">
          <span aria-hidden="true">&laquo;</span>
          <span class="sr-only">Назад</span>
        </a>
      </li>
    }
    @if (Model.Paginator.PreviousPageUrl == null)
    {
      <li class="page-item disabled">
        <a class="page-link" href="#" aria-label="Назад">
          <span aria-hidden="true">&laquo;</span>
          <span class="sr-only">Назад</span>
        </a>
      </li>
    }
    @for (int i = 1; i <= Model.Paginator.TotalPages; i++)
    {
      var link = i == 1 ? "/ru/blog/" : "/ru/blog/page/" + i.ToString() + "/";
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
        <a class="page-link" href='@Model.Paginator.NextPageUrl.Replace("index.html", "")' aria-label="Вперёд">
          <span aria-hidden="true">&raquo;</span>
          <span class="sr-only">Вперёд</span>
        </a>
      </li>
    }
    @if (Model.Paginator.NextPageUrl == null)
    {
      <li class="page-item disabled">
        <a class="page-link" href="#" aria-label="Вперёд">
          <span aria-hidden="true">&raquo;</span>
          <span class="sr-only">Вперёд</span>
        </a>
      </li>
    }
  </ul>
</nav>

<hr />