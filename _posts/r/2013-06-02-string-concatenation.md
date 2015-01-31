---
layout: post
title: "Конкатенация строк в R"
date: '2013-06-03T03:15:00.000+07:00'
author: Andrey Akinshin
category: R
tags:
- R
- R-strings
- R-operators
modified_time: '2013-06-03T03:15:18.362+07:00'
blogger_id: tag:blogger.com,1999:blog-8501021762411496121.post-3611563057161151608
blogger_orig_url: http://aakinshin.blogspot.com/2013/06/r-string-concatenation.html
---

Давайте поговорим о конкатенации строк. Новички в R пытаются пробовать стандартную конструкцию

~~~ r
"a" + "b"
~~~

Но их постигает неудача — R так не работает. Оператор плюс — это [арифметическая операция](http://stat.ethz.ch/R-manual/R-patched/library/base/html/Arithmetic.html), её нельзя применять к строкам. В R полагается использовать функцию [paste](http://stat.ethz.ch/R-manual/R-patched/library/base/html/paste.html):

~~~ r
paste (..., sep = " ", collapse = NULL)
paste0(..., collapse = NULL)
~~~

<!--more-->

Функция paste соединяет строки, разделяя их некоторым сепаратором sep (который по умолчанию равен пробелу), а paste0 — это её аналог с пустым оператором. Поясним примером:

~~~ r
paste("a", "b", "c") # "a b c"
paste0("a", "b", "c") # "abc"
~~~

Такой способ является стандартным, но многим он не по душе. Хочется иметь какой-нибудь бинарный оператор. Но ведь не обязательно это должен быть плюс (например, в PHP для конкатенации строк [используется точка](http://php.net/manual/ru/language.operators.string.php)). Но давайте будем придерживаться R-стилистики и создадим свой оператор "%+%", который будет складывать строки. Сделать это очень просто:

~~~ r
"%+%" <- function(...){
  paste0(...)
}
~~~

Теперь мы можем складывать строки, используя наш новый бинарный оператор:

~~~ r
"a" %+% "b" %+% "c" # "abc"
~~~

Кроме того, этот оператор будет прекрасно работать и с векторами в лучших традициях R:

~~~ r
"a" %+% "b" %+% 1:3 # "ab1" "ab2" "ab3"
~~~

### Ссылки

* [A handy concatenation operator](http://ctszkin.com/2013/02/12/a-handy-concatenatio-operator/)