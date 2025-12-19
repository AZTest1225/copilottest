# Partner 管理系统

这是一个 Partner 管理系统的代码仓库骨架，前端使用 React + LESS（fetch 调用 API），后端使用 C# ASP.NET MVC，数据库为 PostgreSQL（Code-First）。

目录结构（初始）：
- backend/  — 后端相关说明与代码（ASP.NET MVC）
- frontend/ — 前端相关说明与代码（React + LESS）
- cpplan.md — 实现计划

快速开始（建议步骤）：

1. 创建后端项目（在 `backend/` 下）

```bash
cd backend
# 使用 ASP.NET MVC 模板（示例）
dotnet new mvc -n PartnerManager.Api -o src/PartnerManager.Api
cd src/PartnerManager.Api
# 添加 EF Core PostgreSQL provider
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
```

2. 创建前端项目（在 `frontend/` 下）

```bash
cd frontend
# 使用 Vite 创建 React 项目（或使用 CRA）
npm create vite@latest . -- --template react
npm install
# 安装 less 支持
npm install --save-dev less
```

3. 运行数据库迁移与种子（后端）

```bash
# 在后端项目中
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. 启动服务

```bash
# 后端
dotnet run --project backend/src/PartnerManager.Api/PartnerManager.Api.csproj
# 前端（开发）
cd frontend
npm run dev
```

更多实施细节见 `cpplan.md` 和 `backend/README.md`、`frontend/README.md`。