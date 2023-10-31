﻿using Flunt.Notifications;

namespace IWantApp.Domain;

public abstract class Entity: Notifiable<Notification> // Notifications é do Flunt
{
    public Entity()
    {
        Id = Guid.NewGuid(); // gera um ID ao instanciar a classe.
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string EditedBy { get; set; }
    public DateTime EditedOn { get; set; }
}
