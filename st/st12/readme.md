# 数据库操作

```sql
CREATE TABLE [dbo] . [ToDos] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name ] NVARCHAR(255) NOT NULL,
    [Deadline] DATETIME NOT NULL,
    [Completed] BIT NOT NULL
)
```

## LINQ(Language Integrated Query)

中文: 语言集成查询

Integrated: 集成的
