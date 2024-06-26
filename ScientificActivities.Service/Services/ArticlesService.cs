﻿using ScientificActivities.Data.Enums;
using ScientificActivities.Data.Models.Publication;
using ScientificActivities.Service.CustomException;
using ScientificActivities.Service.ModelRequest.Publication;
using ScientificActivities.Service.Services.Interface.Providers;
using ScientificActivities.Service.Services.Interface.Providers.Parsers;
using ScientificActivities.Service.Services.Interface.Services;
using ScientificActivities.StorageEnums;

namespace ScientificActivities.Service.Services;

public class ArticlesService : IArticlesService
{
    private readonly IArticlesProvider _articlesProvider;
    private readonly IJournalProvider _journalProvider;
    private readonly IArticleParseProvider _articleParseProvider;

    public ArticlesService(IArticlesProvider articlesProvider, IJournalProvider journalProvider,
        IArticleParseProvider articleParseProvider)
    {
        _articlesProvider = articlesProvider;
        _journalProvider = journalProvider;
        _articleParseProvider = articleParseProvider;
    }


    public async Task<Guid> CreateAsync(ArticlesRequest entityRequest, CancellationToken cancellationToken)
    {
        var existingArticle = await _articlesProvider.FindAsync(entityRequest.Name, cancellationToken);
        if (existingArticle != null)
            return existingArticle.Id;
        //throw new ExistIsEntityException("Такая статья уже существует");


        Journal journal;
        if (string.IsNullOrWhiteSpace(entityRequest.JounalName))
        {
            journal = await _journalProvider.FindAsync(entityRequest.JournalId, cancellationToken);
            if (journal == null)
                throw new MissingDivisionException("Такой статьи не существует");
        }
        else
        {
            journal = await _journalProvider.FindAsync(entityRequest.JounalName, cancellationToken);
            if (journal == null)
                throw new MissingDivisionException("Такой статьи не существует");
        }

        var articlesDb = new Article(entityRequest.Name,
            entityRequest.Number,
            entityRequest.Year,
            entityRequest.Pages,
            (EnumRSCI)Enum.Parse(typeof(EnumRSCI), entityRequest.Rsci, true),
            (EnumVAK)Enum.Parse(typeof(EnumVAK), entityRequest.Vak, true),
            (EnumCoreRSCI)Enum.Parse(typeof(EnumCoreRSCI), entityRequest.CoreRsci, true),
            entityRequest.Volume,
            entityRequest.Language,
            journal);
        await _articlesProvider.AddAsync(articlesDb, cancellationToken);
        return articlesDb.Id;
    }

    public async Task<Guid> ParseAsync(string url, CancellationToken cancellationToken)
    {
        var entityRequest = await _articleParseProvider.ParseAsync(url, cancellationToken);

        if (await _articlesProvider.FindAsync(entityRequest.Name, cancellationToken) != null)
            throw new ExistIsEntityException("Такая статья уже существует");
        var journal = await _journalProvider.FindAsync(entityRequest.JournalId, cancellationToken);
        if (journal == null)
            throw new MissingDivisionException("Такого журнала не существует");
        var articlesDb = new Article(entityRequest.Name,
            entityRequest.Number,
            entityRequest.Year,
            entityRequest.Pages,
            (EnumRSCI)Enum.Parse(typeof(EnumRSCI), entityRequest.Rsci, true),
            (EnumVAK)Enum.Parse(typeof(EnumVAK), entityRequest.Vak, true),
            (EnumCoreRSCI)Enum.Parse(typeof(EnumCoreRSCI), entityRequest.CoreRsci, true),
            entityRequest.Volume,
            entityRequest.Language,
            journal);
        await _articlesProvider.AddAsync(articlesDb, cancellationToken);
        return articlesDb.Id;
    }

    public async Task<Article?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var articles = await _articlesProvider.FindAsync(id, cancellationToken);
        if (articles == null)
            throw new NotExistException("Такой статьи не существует");
        return articles;
    }

    public async Task<Article> GetAsync(string name, CancellationToken cancellationToken)
    {
        var articles = await _articlesProvider.FindAsync(name, cancellationToken);
        if (articles == null)
            throw new NotExistException("Такой статьи не существует");
        return articles;
    }

    public async Task<Article> UpdateAsync(ArticlesRequest entityRequest, CancellationToken cancellationToken)
    {
        var articlesDb = await GetAsync(entityRequest.Id, cancellationToken);
        var journal = await _journalProvider.FindAsync(entityRequest.JournalId, cancellationToken);
        if (journal == null)
            throw new MissingDirectionException("Такого журнала не существует");
        
        articlesDb.Name = entityRequest.Name;
        articlesDb.Number = entityRequest.Number;
        articlesDb.Year = entityRequest.Year;
        articlesDb.Pages = entityRequest.Pages;
        articlesDb.Rsci = (EnumRSCI)Enum.Parse(typeof(EnumRSCI), entityRequest.Rsci, true);
        articlesDb.Vak = (EnumVAK)Enum.Parse(typeof(EnumVAK), entityRequest.Vak, true);
        articlesDb.UpdateChange = DateTime.Now;
        
        await _articlesProvider.UpdateAsync(articlesDb, cancellationToken);
        return articlesDb;
    }


    public async Task<List<Article>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _articlesProvider.GetAllAsync(new CancellationToken());
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _articlesProvider.DeleteAsync(id, cancellationToken);
    }
}