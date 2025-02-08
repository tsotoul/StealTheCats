IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Cats] (
    [Id] int NOT NULL IDENTITY,
    [CatId] nvarchar(max) NOT NULL,
    [Width] int NOT NULL,
    [Height] int NOT NULL,
    [Image] nvarchar(max) NOT NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_Cats] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Tags] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CatTags] (
    [Id] int NOT NULL IDENTITY,
    [CatId] int NOT NULL,
    [TagId] int NOT NULL,
    CONSTRAINT [PK_CatTags] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CatTags_Cats_CatId] FOREIGN KEY ([CatId]) REFERENCES [Cats] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CatTags_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_CatTags_CatId] ON [CatTags] ([CatId]);
GO

CREATE INDEX [IX_CatTags_TagId] ON [CatTags] ([TagId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250208134329_initialCreate', N'8.0.10');
GO

COMMIT;
GO

