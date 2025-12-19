# 后端（ASP.NET MVC）

说明：
本目录用于存放后端 ASP.NET MVC 项目（建议项目名 `PartnerManager.Api`）。使用 EF Core (Code-First) 与 Npgsql 连接 PostgreSQL。

建议初始化命令：

```bash
mkdir -p backend/src
cd backend/src
dotnet new mvc -n PartnerManager.Api -o PartnerManager.Api
cd PartnerManager.Api
# 添加依赖
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
# 初始化 gitignore（已在仓库根）
```

配置建议：
- 使用 `AppSettings.json` 保存连接字符串（在生产环境使用环境变量覆盖）
- 在 `Startup`/`Program.cs` 中配置 JWT 验证、EF Core DbContext、依赖注入
- 分层：Controllers -> Services -> Repositories -> Data

数据库迁移示例：

```bash
# 生成迁移
dotnet ef migrations add InitialCreate
# 应用迁移
dotnet ef database update
```

性能建议（支持 1k 并发）：
- 配置 Kestrel 并调整线程池
- 使用连接池并添加必要索引
- 使用 Redis/Memory 缓存热点数据

后续：我将建立基础的 DbContext、实体和初始迁移（如你确认我继续）。