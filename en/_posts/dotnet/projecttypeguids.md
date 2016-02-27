---
layout: post
title: "Visual Studio and ProjectTypeGuids.cs"
date: "2016-02-27"
categories: ["dotnet"]
tags:
- ".NET"
- C#
- VisualStudio
- Hate
---

It's a story about how I tried to open a project in Visual Studio for a few hours. The other day, I was going to do some work. I pulled last commits from a repo, opened Visual Studio, and prepared to start coding. However, one of a project in my solution failed to open with a strange message:

```
error  : The operation could not be completed.
```

In the Solution Explorer, I had *"load failed"* as a project status and the following message instead of the file tree: *"The project requires user input. Reload the project for more information."* Hmm, ok, I reloaded the project and got a few more errors:

```
error  : The operation could not be completed.
error  : The operation could not be completed.
```
<!--more-->

Long story short, I did the following things:

* `del /s *.suo *.user`
* `git clean -xfd`
* `shutdown -r`

Northing helps. Then I started to investigate which commit broke project loading. Long live `git bisect`, the commit was found. But it did not contain anything suspicious. Just a new unremarkable file called `ProjectTypeGuids.cs` and the following file in `.csproj`:

```xml
<Compile Include="ProjectTypeGuids.cs" />
```

What can be possible wrong with such commit? Further investigation revealed the following non-obvious fact: Visual Studio can't load a project that contains the `ProjectTypeGuids.cs`. I'm serious. Try it yourself:

1. Open Visual Studio (2013 or 2015).
2. Create a console application or a class library.
3. Add new file: `ProjectTypeGuids.cs`.
4. Save all.
5. Close the solution.
6. Try to open this solution.

I still don't know why Visual Studio can't do it. I just renamed this file in my project and now everything works fine.

By the way, [Rider](https://blog.jetbrains.com/dotnet/2016/01/13/project-rider-a-csharp-ide/) open such projects very well. =)

### See also

* [Happy Monday!](/en/blog/dotnet/happy-monday/)