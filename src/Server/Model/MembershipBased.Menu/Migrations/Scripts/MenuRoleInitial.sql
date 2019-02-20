begin tran
IF schema_id('menu') IS NULL
    EXECUTE('CREATE SCHEMA [menu]')
IF schema_id('AspNet') IS NULL
    EXECUTE('CREATE SCHEMA [AspNet]')
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[menu].[MenuItemRelationGroups]') AND type in (N'U'))
begin
CREATE TABLE [menu].[MenuItemRelationGroups] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_menu.MenuItemRelationGroups] PRIMARY KEY ([Id])
)
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[menu].[MenuItemRelationRouteValues]') AND type in (N'U'))
begin
CREATE TABLE [menu].[MenuItemRelationRouteValues] (
    [MenuItemRelationId] [int] NOT NULL,
    [RouteParamId] [int] NOT NULL,
    [Value] [nvarchar](max),
    CONSTRAINT [PK_menu.MenuItemRelationRouteValues] PRIMARY KEY ([MenuItemRelationId], [RouteParamId])
)
CREATE INDEX [IX_MenuItemRelationId] ON [menu].[MenuItemRelationRouteValues]([MenuItemRelationId])
CREATE INDEX [IX_RouteParamId] ON [menu].[MenuItemRelationRouteValues]([RouteParamId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[menu].[MenuItemRelations]') AND type in (N'U'))
begin
CREATE TABLE [menu].[MenuItemRelations] (
    [Id] [int] NOT NULL IDENTITY,
    [RouteNameId] [int] NOT NULL,
    [MenuItemId] [int] NOT NULL,
    [MenuItemRelationGroupId] [int] NOT NULL,
    [IsDocked] [bit] NOT NULL,
    [RoleId] [int],
    [UserId] [int],
    [Discriminator] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_menu.MenuItemRelations] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_RouteNameId] ON [menu].[MenuItemRelations]([RouteNameId])
CREATE INDEX [IX_MenuItemId] ON [menu].[MenuItemRelations]([MenuItemId])
CREATE INDEX [IX_MenuItemRelationGroupId] ON [menu].[MenuItemRelations]([MenuItemRelationGroupId])
CREATE INDEX [IX_RoleId] ON [menu].[MenuItemRelations]([RoleId])
CREATE INDEX [IX_UserId] ON [menu].[MenuItemRelations]([UserId])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[menu].[MenuItems]') AND type in (N'U'))
begin
CREATE TABLE [menu].[MenuItems] (
    [Id] [int] NOT NULL IDENTITY,
    [ParentId] [int],
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_menu.MenuItems] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ParentId] ON [menu].[MenuItems]([ParentId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[menu].[RouteNames]') AND type in (N'U'))
begin
CREATE TABLE [menu].[RouteNames] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_menu.RouteNames] PRIMARY KEY ([Id])
)
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[Roles]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[Roles] (
    [Id] [int] NOT NULL IDENTITY,
    [InRoleId] [int],
    [Scope] [nvarchar](max),
    [Name] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_AspNet.Roles] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_InRoleId] ON [AspNet].[Roles]([InRoleId])
CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNet].[Roles]([Name])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[UserRoles]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[UserRoles] (
    [UserId] [int] NOT NULL,
    [RoleId] [int] NOT NULL,
    CONSTRAINT [PK_AspNet.UserRoles] PRIMARY KEY ([UserId], [RoleId])
)
CREATE INDEX [IX_UserId] ON [AspNet].[UserRoles]([UserId])
CREATE INDEX [IX_RoleId] ON [AspNet].[UserRoles]([RoleId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[Users]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[Users] (
    [Id] [int] NOT NULL IDENTITY,
    [Rating] [decimal](18, 2) NOT NULL,
    [ProfilePicturePath] [nvarchar](max),
    [FirstName] [nvarchar](max),
    [LastName] [nvarchar](max),
    [Patronymic] [nvarchar](max),
    [DateUpdated] [datetime],
    [LastActivityDate] [datetime],
    [LastLogin] [datetime],
    [RemindInDays] [int] NOT NULL,
    [DateRegistration] [datetime] NOT NULL,
    [Description] [nvarchar](max),
    [Email] [nvarchar](256),
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar](max),
    [SecurityStamp] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEndDateUtc] [datetime],
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,
    [UserName] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_AspNet.Users] PRIMARY KEY ([Id])
)
CREATE UNIQUE INDEX [UserNameIndex] ON [AspNet].[Users]([UserName])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[UserClaims]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[UserClaims] (
    [Id] [int] NOT NULL IDENTITY,
    [UserId] [int] NOT NULL,
    [ClaimType] [nvarchar](max),
    [ClaimValue] [nvarchar](max),
    CONSTRAINT [PK_AspNet.UserClaims] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UserId] ON [AspNet].[UserClaims]([UserId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[UserLogins]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[UserLogins] (
    [LoginProvider] [nvarchar](128) NOT NULL,
    [ProviderKey] [nvarchar](128) NOT NULL,
    [UserId] [int] NOT NULL,
    CONSTRAINT [PK_AspNet.UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey], [UserId])
)
CREATE INDEX [IX_UserId] ON [AspNet].[UserLogins]([UserId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[menu].[RouteParams]') AND type in (N'U'))
begin
CREATE TABLE [menu].[RouteParams] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_menu.RouteParams] PRIMARY KEY ([Id])
)
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_menu.MenuItemRelationRouteValues_menu.MenuItemRelations_MenuItemRelationId')
		begin
ALTER TABLE [menu].[MenuItemRelationRouteValues] ADD CONSTRAINT [FK_menu.MenuItemRelationRouteValues_menu.MenuItemRelations_MenuItemRelationId] FOREIGN KEY ([MenuItemRelationId]) REFERENCES [menu].[MenuItemRelations] ([Id]) 
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_menu.MenuItemRelationRouteValues_menu.RouteParams_RouteParamId')
		begin
ALTER TABLE [menu].[MenuItemRelationRouteValues] ADD CONSTRAINT [FK_menu.MenuItemRelationRouteValues_menu.RouteParams_RouteParamId] FOREIGN KEY ([RouteParamId]) REFERENCES [menu].[RouteParams] ([Id]) 
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_menu.MenuItemRelations_menu.MenuItems_MenuItemId')
		begin
ALTER TABLE [menu].[MenuItemRelations] ADD CONSTRAINT [FK_menu.MenuItemRelations_menu.MenuItems_MenuItemId] FOREIGN KEY ([MenuItemId]) REFERENCES [menu].[MenuItems] ([Id]) 
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_menu.MenuItemRelations_menu.MenuItemRelationGroups_MenuItemRelationGroupId')
		begin
ALTER TABLE [menu].[MenuItemRelations] ADD CONSTRAINT [FK_menu.MenuItemRelations_menu.MenuItemRelationGroups_MenuItemRelationGroupId] FOREIGN KEY ([MenuItemRelationGroupId]) REFERENCES [menu].[MenuItemRelationGroups] ([Id]) 
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_menu.MenuItemRelations_menu.RouteNames_RouteNameId')
		begin
ALTER TABLE [menu].[MenuItemRelations] ADD CONSTRAINT [FK_menu.MenuItemRelations_menu.RouteNames_RouteNameId] FOREIGN KEY ([RouteNameId]) REFERENCES [menu].[RouteNames] ([Id]) 
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_menu.MenuItemRelations_AspNet.Roles_RoleId')
		begin
ALTER TABLE [menu].[MenuItemRelations] ADD CONSTRAINT [FK_menu.MenuItemRelations_AspNet.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNet].[Roles] ([Id])
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_menu.MenuItemRelations_AspNet.Users_UserId')
		begin
ALTER TABLE [menu].[MenuItemRelations] ADD CONSTRAINT [FK_menu.MenuItemRelations_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id])
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_menu.MenuItems_menu.MenuItems_ParentId')
		begin
ALTER TABLE [menu].[MenuItems] ADD CONSTRAINT [FK_menu.MenuItems_menu.MenuItems_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [menu].[MenuItems] ([Id])
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.Roles_AspNet.Roles_InRoleId')
		begin
ALTER TABLE [AspNet].[Roles] ADD CONSTRAINT [FK_AspNet.Roles_AspNet.Roles_InRoleId] FOREIGN KEY ([InRoleId]) REFERENCES [AspNet].[Roles] ([Id])
end 
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserRoles_AspNet.Roles_RoleId')
		begin
ALTER TABLE [AspNet].[UserRoles] ADD CONSTRAINT [FK_AspNet.UserRoles_AspNet.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNet].[Roles] ([Id]) 
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserRoles_AspNet.Users_UserId')
		begin
ALTER TABLE [AspNet].[UserRoles] ADD CONSTRAINT [FK_AspNet.UserRoles_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end 
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserClaims_AspNet.Users_UserId')
		begin
ALTER TABLE [AspNet].[UserClaims] ADD CONSTRAINT [FK_AspNet.UserClaims_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end 
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserLogins_AspNet.Users_UserId')
		begin
ALTER TABLE [AspNet].[UserLogins] ADD CONSTRAINT [FK_AspNet.UserLogins_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end 

commit tran