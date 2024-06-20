# 简码自动装填工具

有了某些**字词**的**全码**，[统计](https://github.com/GarthTB/WordFreqCounter)或者[爬取](https://github.com/GarthTB/BCCFreqSpider)到了**词频**，简码就可以轻易生成了。

输入文件须为UTF-8编码。每一行的格式为`字词\t编码\t优先级（词频）`。

输出文件总共有三个：

- `_SimplifiedOnly.txt`：简化得到的所有新码
- `_RemainsOnly.txt`：没法简化的剩余码
- `_Full.txt`：包含简码、保留全码的总文件

## 控制台参数：

1. 字词-编码-优先级文件路径
2. 最短码长（0-255）
