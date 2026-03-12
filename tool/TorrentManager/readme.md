# TorrentManager

基于 SQLite 管理 `.fastresume` 文件的命令行工具。

## 说明

- `.fastresume`：qBittorrent 的快速恢复文件

## 数据结构

| 字段              | 类型   | 说明               |
| ----------------- | ------ | ------------------ |
| `TOR_HASH`        | string | 种子哈希值（主键） |
| `fastresume_file` | blob   | 快速恢复文件       |
| `qbt_category`    | string | qB 分类            |
| `save_path`       | string | 保存路径           |

## 命令总览

```bash
dotnet run -- <command> [options]
```

### 全局选项

- `--db <database_path>`：指定数据库文件路径
- `--path <export_directory>`：指定导出目录（仅 `export` 使用）

默认情况下，`--db` 未提供时使用 `./fastresume.db`。

## 1. 添加 `.fastresume` 文件

### 命令示例

```bash
dotnet run -- add     <path_to_fastresume_file>
dotnet run -- add_all <path_to_fastresume_files_directory>
```

### 处理逻辑

1. 读取 `.fastresume` 文件内容
2. 提取字段：
	- `info-hash`：种子哈希
	- `qBt-category`：qB 分类
	- `save_path`：保存路径
3. 将解析结果写入 SQLite

## 2. 导出 `.fastresume` 文件

### 命令示例

```bash
dotnet run -- export <by_category | by_save_path> <pattern> [--path <export_directory>] [--db <database_path>]
```

支持模糊匹配，例如 `A%` 可匹配所有以 `A` 开头的分类或保存路径。

### 处理逻辑

1. 根据 `<by_category | by_save_path>` 选择匹配字段
2. 使用 `<pattern>` 执行模糊匹配查询
3. 将匹配记录的 `fastresume_file` 写入目标目录，命名为 `<TOR_HASH>.fastresume`
4. 输出导出数量和目标路径

## 3. 更新分类或保存路径

### 命令示例

```bash
dotnet run -- update <by_category | by_save_path> <pattern> <replace <search_str> <replace_str> | <new_value>> [options]
```

### 两种更新模式

- 直接覆盖为新值：

```bash
dotnet run -- update by_category <pattern> <new_value>
dotnet run -- update by_save_path <pattern> <new_value>
```

- 子串替换（`replace` 模式）：

```bash
dotnet run -- update by_category <pattern> replace <search_str> <replace_str>
dotnet run -- update by_save_path <pattern> replace <search_str> <replace_str>
```

### 处理逻辑

1. 根据 `<by_category | by_save_path>` 选择匹配字段
2. 使用 `<pattern>` 查询匹配记录
3. 按模式更新 `fastresume_file` 中目标字段值：
	- `replace` 模式：将字段值中的 `search_str` 替换为 `replace_str`
	- 直接替换模式：将字段值整体替换为 `<new_value>`
4. 将更新后的记录写回数据库

## 模块化测试

测试项目：`tool/TorrentManager.Tests`

```bash
dotnet test ./tool/TorrentManager.Tests/TorrentManager.Tests.csproj
```

当前覆盖模块：

1. `Cli.CommandLineParser`：参数解析与错误处理
2. `Data.TorrentRepository`：`by_category` / `by_save_path` 查询逻辑
3. `TorrentApp`：`export` / `update` 主流程与异常路径

## 常用示例

```bash
# 导出
dotnet run -- export by_category music% --path _exported_music
dotnet run -- export by_save_path %\music\% --path _exported\music

# 直接替换
dotnet run -- update by_category music/2026-03-07/tmp music/2026-03-07/tmp2
dotnet run -- update by_save_path D:\qb\Downloads\music\2026-03-07\tmp D:\qb\Downloads\music\2026-03-07\tmp2

# 子串替换
dotnet run -- update by_save_path %\tmp replace tmp tmp2

# 校验导出
dotnet run -- export by_save_path %tmp2 --path _exported\tmp2
```