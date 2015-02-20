---
layout: ru-post
title: "Ещё раз об экранировании URI в .NET"
date: '2014-11-21T22:32:00.000+06:00'
categories: ["ru", "dotnet"]
tags:
- ".NET"
- URI
- Escaping
- C#
modified_time: '2014-11-21T22:43:01.699+06:00'
blogger_id: tag:blogger.com,1999:blog-8501021762411496121.post-6099756098875806923
blogger_orig_url: http://aakinshin.blogspot.com/2014/11/dotnet-uri-escaping-2.html
---

Сегодня на StackOverflow мне попался интересный вопрос: [«Unit test ReSharper and NUnit give different results»](http://stackoverflow.com/q/27062562/184842). Суть заключалась в том, что ReSharper и NUnit дают разные результаты при экранировании URI. Я решил немножко углубиться в эту проблему. Сегодняшний пост продолжает недавно начатую мной тему [«Об экранировании слеша в .NET»]({% post_url dotnet/2014-11-13-uri-slash-escape %}). Разобраться с проблемой нам поможет следующая небольшая программка:

```cs
public void Run()
{
    Print("http://localhost/a%2Fb");
    Print("http://localhost/a.?b");
    Print("http://localhost/a?b=c%3Fd%3De");
}

public void Print(string uriString)
{
    var uri = new Uri(uriString);
    Console.WriteLine("Original: " + uri.OriginalString);
    Console.WriteLine("Absolute: " + uri.AbsoluteUri);
    Console.WriteLine("ToString: " + uri.ToString());
    Console.WriteLine();
}
```
<!--more-->

Я взял на рассмотрение три строки: с экранированным слешом, с точкой и с экранированными вопросом и знаком равенства. Программа запускалась под MS.NET 4.0, MS.NET 4.0 с опцией `genericUriParserOptions="DontUnescapePathDotsAndSlashes"`, MS.NET 4.5, Mono 3.2.8, Mono 3.10.0. Результаты:

```cs
// MS.NET 4.0:

Original: http://localhost/a%2Fb
Absolute: http://localhost/a/b
ToString: http://localhost/a/b

Original: http://localhost/a.?b
Absolute: http://localhost/a?b
ToString: http://localhost/a?b

Original: http://localhost/a?b=c%3Fd%3De
Absolute: http://localhost/a?b=c%3Fd%3De
ToString: http://localhost/a?b=c?d=e

// MS.NET 4.0 (DontUnescapePathDotsAndSlashes):

Original: http://localhost/a%2Fb
Absolute: http://localhost/a%2Fb
ToString: http://localhost/a/b

Original: http://localhost/a.?b
Absolute: http://localhost/a?b
ToString: http://localhost/a?b

Original: http://localhost/a?b=c%3Fd%3De
Absolute: http://localhost/a?b=c%3Fd%3De
ToString: http://localhost/a?b=c?d=e

// MS.NET 4.5:

Original: http://localhost/a%2Fb
Absolute: http://localhost/a%2Fb
ToString: http://localhost/a%2Fb

Original: http://localhost/a.?b
Absolute: http://localhost/a.?b
ToString: http://localhost/a.?b

Original: http://localhost/a?b=c%3Fd%3De
Absolute: http://localhost/a?b=c%3Fd%3De
ToString: http://localhost/a?b=c%3Fd%3De

// Mono 3.2.8

Original: http://localhost/a%2Fb
Absolute: http://localhost/a/b
ToString: http://localhost/a/b

Original: http://localhost/a.?b
Absolute: http://localhost/a.?b
ToString: http://localhost/a.?b

Original: http://localhost/a?b=c%3Fd%3De
Absolute: http://localhost/a?b=c%3Fd%3De
ToString: http://localhost/a?b=c%3Fd=e

// Mono 3.10

Original: http://localhost/a%2Fb
Absolute: http://localhost/a%2Fb
ToString: http://localhost/a%2Fb

Original: http://localhost/a.?b
Absolute: http://localhost/a.?b
ToString: http://localhost/a.?b

Original: http://localhost/a?b=c%3Fd%3De
Absolute: http://localhost/a?b=c%3Fd%3De
ToString: http://localhost/a?b=c%3Fd%3De
```

Внимательное созерцание результатов может подтолкнуть к следующим выводам:

* `uri.AbsoluteUri` совсем не обязательно совпадает с `uri.ToString()`. Например, `DontUnescapePathDotsAndSlashes` хак в MS.NET 4.0 при экранировании слеша влияет на `AbsoluteUri`, но не оказывает влияния на `ToString()`. Под Mono 3.2.8 можно увидеть проблемы с экранированным знаком равенства в `AbsoluteUri`.
* В Mono и MS.NET были разные проблемы с обработкой URI. Например, под MS.NET 4.0 строка `a.?b` превратилась в `a?b`, а под Mono 3.2.8 мы увидели всё тот же `a.?b`.
* В последних версиях (MS.NET 4.5 и Mono 3.10) всё хорошо: `AbsoluteUri` и `ToString()` на *приведённых примерах* совпадают с `OriginalString`.

Что касается изначального StackOverflow-вопроса, то ReSharper проводит тестирование правильно: он запускает тесты под нужную версию .NET. А NUnit при консольном тестировании без указания специфических параметров по каким-то причинам может подхватить логику из старых библиотек.

### Выводы

Если вы работаете с URI, в который могут попасть разные спецсимволы в экранированном или явном виде, то лучше бы вам не полагаться на стандартную реализацию по обработке исходной строчки: результат работы `AbsoluteUri` и `ToString()`	могут вас неприятно удивить. Если вы уверены, что у вас повсеместно используется MS.NET 4.5+ или Mono 3.10+, то скорее всего у вас всё будет нормально, но при поддержке старых версий .NET лучше бы написать свою логику по работе с URI.