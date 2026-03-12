# TorrentManager

种子管理器

基于 SQLite 管理 `.fastresume` 文件

## 说明

- `.fastresume`: qB 快速恢复文件

## SQLite

| 字段              | 类型   | 说明               |
| ----------------- | ------ | ------------------ |
| `TOR_HASH`        | string | 种子哈希值（主键） |
| `fastresume_file` | blob   | 快速恢复文件       |
| `qbt_category`    | string | qB分类             |
| `save_path`       | string | 保存路径           |

## 操作

```
dotnet run -- <command> [options] [--db <database_path>]
```

如果 `--db` 参数未提供，默认使用 `./fastresume.db` 作为数据库文件

### 1. 添加 `.fastresume` 文件

命令行示例:

```
dotnet run -- add     <path_to_fastresume_file>            # 添加单个文件
dotnet run -- add_all <path_to_fastresume_files_directory> # 添加目录下所有文件
```

功能实现:

1. 读取 `.fastresume` 文件内容
2. 提取文件指定字段:
   - `info-hash`: 从文件内容中提取种子哈希值
   - `qBt-category`: 从文件内容中提取 qB 分类
   - `save_path`: 从文件内容中提取保存路径
3. 将提取的数据存储到 SQLite 数据库中

### 2. 导出 `.fastresume` 文件

命令行示例:

```
dotnet run -- export <by_category | by_save_path> <pattern> [--path <export_directory>] [--db <database_path>] # 导出指定分类的文件
```

支持模糊匹配名称，例如 `A%` 可以匹配所有以 `A` 开头的分类或者保存路径

逻辑:

1. 根据`<by_category | by_save_path>`确定是根据分类还是保存路径进行查询
2. 使用`<pattern>`进行模糊匹配查询数据库，获取符合条件的记录
3. 将查询结果中的 `fastresume_file` 字段内容写入到指定的导出目录中，文件命名为 `<TOR_HASH>.fastresume`
4. 导出完成后，输出导出的文件数量和目标路径

## 测试

```
dotnet run -- export by_category music% --path _exported_music
dotnet run -- export by_save_path %\music\% --path _exported\music
```