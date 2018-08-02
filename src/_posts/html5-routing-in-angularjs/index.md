---
title: HTML5 routing в Angular.js
slug: html5-routing-in-angularjs
date: 20-01-2016
---

Наскоро ми се наложи да използвам HTML5 routing в Angular.js и най-главният проблем
който имах е конфигурацията на Node.js сървъра, тоест да прехвърля стандартна REST
заявка не към Node.js т.е сървъра, а към Angular.js и съответно да бъде обработена от него.
Аз направих следното за всички заявки, Node.js да ги праща към Angular.js, на помощ тук идва 
метода *all*.

```javascript
app.all("/*", function (req, res) {
    res.render("index");
});
```

Съответно след конфигурация на Node.js приложението, вече без проблем можем да използваме HTML5 routing в Angular.js.