---
layout: post
title: "Операторы +=, -= в R"
date: '2013-06-03T03:59:00.000+07:00'
categories: ["r"]
tags:
- R
- R-assignments
- R-operators
modified_time: '2013-06-03T03:59:09.575+07:00'
blogger_id: tag:blogger.com,1999:blog-8501021762411496121.post-6115535252467257462
blogger_orig_url: http://aakinshin.blogspot.com/2013/06/r-compound-assignment.html
---

Продолжаем писать полезные операторы для языка R. В большинстве современных языков есть операторы +=, -= и т.п., они делают синтаксис более лаконичным. А давайте и в R определим подобные операторы, чтобы вместо

``` r
x <- x + 3
y <- y - 2
```

мы могли бы писать:


``` r
x %+=% 3
y %-=% 2
```

<!--more-->

Да ведь это очень просто. Новые операторы можно определить буквально в пару строк:

```
'%+=%' <- function(x, y) {
  mapply(assign, as.character(substitute(x)), x + y, MoreArgs = list(envir = parent.frame()))
  invisible()
}
'%-=%' <- function(x, y) {
  mapply(assign, as.character(substitute(x)), x - y, MoreArgs = list(envir = parent.frame()))
  invisible()
}
```

Теперь можно писать такой вот код:

```
a <- 3
a %+=% 2 # a <- a + 2
a # 5
a %-=% 1 # a <- a - 1
a # 4
```

Разумеется, ничего не мешает определить по аналогии %*=%, %/=% и тому подобные замечательные операторы.