﻿using HtmlAgilityPack;

namespace Parsers;

public class WebSettings
{
    
    // Создаем объект HtmlWeb для загрузки HTML-кода страницы
    HtmlWeb web = new HtmlWeb();

    // Добавляем пользовательский агент и пустые куки
    web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
    web.PreRequest += request =>
    {
        request.CookieContainer = new System.Net.CookieContainer();
    
        // Добавление стандартных заголовков браузера
        request.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        request.Headers["Accept-Encoding"] = "gzip, deflate, br";
        request.Headers["Accept-Language"] = "en-US,en;q=0.5";
        request.Headers["Connection"] = "keep-alive";
        request.Headers["Upgrade-Insecure-Requests"] = "1";
        request.Headers["Referer"] = "https://developer.mozilla.org/ru/docs/Web/JavaScript"; // установите корректный реферер
        request.Headers["Cache-Control"] = "max-age=0";
    
        // Добавление заголовков для имитации активности пользователя
        request.Headers["DNT"] = "1"; // Do Not Track
        request.Headers["TE"] = "Trailers";

        // Дополнительные параметры для повышения аутентичности
        var cookie = new System.Net.Cookie("example_cookie_name", "example_cookie_value", "/", "example.com");
        request.CookieContainer.Add(cookie);

        return true;
    };
}