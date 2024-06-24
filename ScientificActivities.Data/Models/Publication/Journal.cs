﻿using ScientificActivities.StorageEnums;

namespace ScientificActivities.Data.Models.Publication;

public class Journal : BaseModel
{
    public Journal(string name, PublishingHouse publishingHouse, EnumJournalStatus status)
    {
        Name = name;
        PublishingHouse = publishingHouse;
        Status = status;
        Articles = new List<Article>();
    }

    protected Journal()
    {
    }


    public string Name { get; set; } = string.Empty;

    public PublishingHouse? PublishingHouse { get; set; }
    public EnumJournalStatus Status { get; set; }
    
    public List<Article> Articles { get; set; } = new List<Article>();
}