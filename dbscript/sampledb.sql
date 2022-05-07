--建表
CREATE TABLE [dbo].[MainEntity] (
  [Id] bigint  IDENTITY(1,1) NOT NULL,
  [Name] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'名称',
'SCHEMA', N'dbo',
'TABLE', N'MainEntity',
'COLUMN', N'Name'
GO


DBCC CHECKIDENT ('[dbo].[MainEntity]', RESEED, 1)
GO


ALTER TABLE [dbo].[MainEntity] ADD CONSTRAINT [PK_MainEntity] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO



CREATE TABLE [dbo].[Entity1] (
  [Id] bigint  IDENTITY(1,1) NOT NULL,
  [Name] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Type] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [MainId] bigint  NOT NULL
)
GO


EXEC sp_addextendedproperty
'MS_Description', N'名称',
'SCHEMA', N'dbo',
'TABLE', N'Entity1',
'COLUMN', N'Name'
GO

EXEC sp_addextendedproperty
'MS_Description', N'类型',
'SCHEMA', N'dbo',
'TABLE', N'Entity1',
'COLUMN', N'Type'
GO

EXEC sp_addextendedproperty
'MS_Description', N'外键Id',
'SCHEMA', N'dbo',
'TABLE', N'Entity1',
'COLUMN', N'MainId'
GO

DBCC CHECKIDENT ('[dbo].[Entity1]', RESEED, 1)
GO

ALTER TABLE [dbo].[Entity1] ADD CONSTRAINT [PK_Entity1] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO



CREATE TABLE [dbo].[Entity2] (
  [Id] bigint  IDENTITY(1,1) NOT NULL,
  [Name] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Num] int  NOT NULL,
  [MainId] bigint  NOT NULL
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'名称',
'SCHEMA', N'dbo',
'TABLE', N'Entity2',
'COLUMN', N'Name'
GO

EXEC sp_addextendedproperty
'MS_Description', N'个数',
'SCHEMA', N'dbo',
'TABLE', N'Entity2',
'COLUMN', N'Num'
GO

EXEC sp_addextendedproperty
'MS_Description', N'外键Id',
'SCHEMA', N'dbo',
'TABLE', N'Entity2',
'COLUMN', N'MainId'
GO


DBCC CHECKIDENT ('[dbo].[Entity2]', RESEED, 1)
GO


ALTER TABLE [dbo].[Entity2] ADD CONSTRAINT [PK_Entity2] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

--数据

INSERT INTO MainEntity ( name) values ('mato')

INSERT INTO Entity1( name, Type, MainId) values ('abc', 'type1',1)
INSERT INTO Entity1( name, Type, MainId) values ('xyz', 'type2',1)

INSERT INTO Entity2( name, Num, MainId) values ('foo',3,1)
INSERT INTO Entity2( name, Num, MainId) values ('bar',5,1)

SELECT * FROM MainEntity
SELECT * FROM Entity1
SELECT * FROM Entity2