---
layout: post
title: "Об экранировании слеша в .NET"
date: '2014-11-13T19:29:00.000+06:00'
author: Andrey Akinshin
category: dotnet
tags:
- ".NET"
- C#
- Mono
- URI
- Escaping
modified_time: '2014-11-13T19:49:04.348+06:00'
blogger_id: tag:blogger.com,1999:blog-8501021762411496121.post-3381922261046413548
blogger_orig_url: http://aakinshin.blogspot.com/2014/11/dotnet-uri-slash-escape.html
---

Загадка на сегодня: что выведет код?

~~~ cs
var uri = new Uri("http://localhost/%2F1");
Console.WriteLine(uri.OriginalString);
Console.WriteLine(uri.AbsoluteUri);
~~~

Правильный ответ: зависит. Давайте немножко поразбираемся.<!--more-->

### Так что же он выведет?
Результат работы зависит от версии .NET:

~~~ cs
// .NET 4.0
http://localhost/%2F1
http://localhost//1
// .NET 4.5
http://localhost/%2F1
http://localhost/%2F1
~~~

### Да как же так-то?

Увы, до версии .NET 4.0 имела место [неприятная бага](https://connect.microsoft.com/VisualStudio/feedback/details/511010/erroneous-uri-parsing-for-encoded-reserved-characters-according-to-rfc-3986) с экранированием слеша (он же `%2F`). В 4.5 её решили пофиксить, чтобы поведение соответствовало [RFC 3986](http://tools.ietf.org/html/rfc3986). Сделали вроде бы правильно, но добавили дополнительной головной боли разработчикам, которые не знают про этот небольшой нюанс: теперь механизм экранирования зависит от версии Framework-а. Лучше всего использовать правильный механизм из .NET 4.5. Но что, если у нас нет .NET 4.5? Имеется путь починить поведение в .NET 4.0. Для этого необходимо добавить в *.config-файл вашего приложения магические строчки:

~~~ xml
<configuration>
  <uri>
    <schemeSettings>
      <add name="http" 
           genericUriParserOptions="DontUnescapePathDotsAndSlashes" />
    </schemeSettings>
  </uri>
</configuration>
~~~

Работает приведённый фокус-покус начиная с .NET 4.0 beta 2. Т.е., скажем, в .NET 3.5 так сделать не получится. Так что придётся крутиться и вертеться. Например, на просторах интернета [можно найти](http://stackoverflow.com/a/784937/184842) вот такой чудо-хак:

~~~ cs
void ForceCanonicalPathAndQuery(Uri uri)
{
  string paq = uri.PathAndQuery; // need to access PathAndQuery
  FieldInfo flagsFieldInfo = typeof(Uri).GetField("m_Flags", 
    BindingFlags.Instance | BindingFlags.NonPublic);
  ulong flags = (ulong) flagsFieldInfo.GetValue(uri);
  flags &= ~((ulong) 0x30); // Flags.PathNotCanonical|Flags.QueryNotCanonical
  flagsFieldInfo.SetValue(uri, flags);
}
~~~

### А что будет в Mono?

В Mono [накосячили](https://bugzilla.xamarin.com/show_bug.cgi?id=16960) точно также. Починка осуществилась совсем недавно с [выходом](http://www.mono-project.com/docs/about-mono/releases/3.10.0/) Mono 3.10.0 в октябре 2014. Так что если вы сидите на последней версии, то у вас уже всё должно быть хорошо. Но как же нам теперь переключаться между старым и новым поведением? Для этих целей в классе `System.Uri` имеется свойство `IriParsing`. Заглянем в [код](https://github.com/mono/mono/blob/mono-3.10.0/mcs/class/System/System/Uri.cs):

~~~ cs
private static bool s_IriParsing;

internal static bool IriParsing {
    get { return s_IriParsing; }
    set { s_IriParsing = value; }
}
~~~

Выставляется свойство следующим образом:

~~~ cs
static Uri ()
{
#if NET_4_5
    IriParsing = true;
#endif

    var iriparsingVar = 
        Environment.GetEnvironmentVariable ("MONO_URI_IRIPARSING");
    if (iriparsingVar == "true")
        IriParsing = true;
    else if (iriparsingVar == "false")
        IriParsing = false;
}
~~~

Т.е. выставить его проще всего через переменную окружения `MONO_URI_IRIPARSING`.

### Заключение

Бага не особо приятная и может стоить вам многих часов душевного спокойствия, если вы на неё случайно наткнётесь. Поэтому я решил оформить такую вот небольшую заметку, чтобы побольше людей было в курсе. Помните о неоднозначности экранирования некоторых URI и пишите стабильный код.

### Ссылки

* [MS Connect 511010: Erroneous URI parsing for encoded, reserved characters, according to RFC 3986](https://connect.microsoft.com/VisualStudio/feedback/details/511010/erroneous-uri-parsing-for-encoded-reserved-characters-according-to-rfc-3986)
* [Mono Bug 16960](https://bugzilla.xamarin.com/show_bug.cgi?id=16960)
* [StackOverflow: Getting a Uri with escaped slashes on mono](http://stackoverflow.com/q/20769150/184842)
* [GETting a URL with an url-encoded slash](http://stackoverflow.com/q/781205/184842">StackOverflow)
* [Mono 3.10.0 release notes](http://www.mono-project.com/docs/about-mono/releases/3.10.0/)
* [Mike Hadlow: How to stop System.Uri un-escaping forward slash characters](http://mikehadlow.blogspot.co.uk/2011/08/how-to-stop-systemuri-un-escaping.html)
* [Arnout's Eclectica: URL-encoded slashes in System.Uri](http://grootveld.com/archives/21/url-encoded-slashes-in-systemuri)