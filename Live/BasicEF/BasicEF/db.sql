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

CREATE TABLE [Hobbies] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_Hobbies] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Mensen] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_Mensen] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [HobbyPerson] (
    [HobbiesId] int NOT NULL,
    [PeopleId] int NOT NULL,
    CONSTRAINT [PK_HobbyPerson] PRIMARY KEY ([HobbiesId], [PeopleId]),
    CONSTRAINT [FK_HobbyPerson_Hobbies_HobbiesId] FOREIGN KEY ([HobbiesId]) REFERENCES [Hobbies] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_HobbyPerson_Mensen_PeopleId] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[Mensen] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_HobbyPerson_PeopleId] ON [HobbyPerson] ([PeopleId]);
GO

CREATE INDEX [IX_Mensen_Name] ON [dbo].[Mensen] ([Name]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221004095933_Init', N'6.0.9');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [HobbyPerson] DROP CONSTRAINT [FK_HobbyPerson_Mensen_PeopleId];
GO

ALTER TABLE [dbo].[Mensen] DROP CONSTRAINT [PK_Mensen];
GO

EXEC sp_rename N'[dbo].[Mensen]', N'People';
DECLARE @defaultSchema sysname = SCHEMA_NAME();
EXEC(N'ALTER SCHEMA [' + @defaultSchema + N'] TRANSFER [dbo].[People];');
GO

EXEC sp_rename N'[People].[IX_Mensen_Name]', N'IX_People_Name', N'INDEX';
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Hobbies]') AND [c].[name] = N'Description');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Hobbies] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Hobbies] ALTER COLUMN [Description] nvarchar(1024) NOT NULL;
ALTER TABLE [Hobbies] ADD DEFAULT N'' FOR [Description];
GO

ALTER TABLE [People] ADD CONSTRAINT [PK_People] PRIMARY KEY ([Id]);
GO

CREATE INDEX [IX_Hobbies_Description] ON [Hobbies] ([Description]);
GO

ALTER TABLE [HobbyPerson] ADD CONSTRAINT [FK_HobbyPerson_People_PeopleId] FOREIGN KEY ([PeopleId]) REFERENCES [People] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221004100257_V1', N'6.0.9');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description') AND [object_id] = OBJECT_ID(N'[Hobbies]'))
    SET IDENTITY_INSERT [Hobbies] ON;
INSERT INTO [Hobbies] ([Id], [Description])
VALUES (-2, N'Kanteklossen'),
(-1, N'Sigarenbandjes');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description') AND [object_id] = OBJECT_ID(N'[Hobbies]'))
    SET IDENTITY_INSERT [Hobbies] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] ON;
INSERT INTO [People] ([Id], [Name])
VALUES (-2, N'Marieke Klaasen'),
(-1, N'Jan de Vries');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221004110433_V2', N'6.0.9');
GO

COMMIT;
GO

