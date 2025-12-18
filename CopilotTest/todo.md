# 项目 TODO 列表（状态快照）

更新时间: 2025-12-18

- [x] 初始化项目及文档
  - 状态: 完成
  - 说明: 已建立仓库根目录、`cpplan.md`、`README.md`、`.gitignore`，并添加 backend/frontend 说明文件和占位 Dockerfile。

- [x] 设计数据模型（Code-First）
  - 状态: 完成
  - 说明: 已实现 `User`、`Partner`、`Activity`、`PartnerActivity` 实体与 `AppDbContext`（文件路径: `backend/src/PartnerManager.Api/Models` 与 `Data/AppDbContext.cs`）。

- [x] 后端骨架（C# ASP.NET MVC）
  - 状态: 完成
  - 说明: 已创建项目文件、`Program.cs`、依赖注入、EF Core 与 Npgsql 配置、Swagger 与 CORS 支持。

- [x] 实现认证与授权
  - 状态: 完成
  - 说明: 已实现注册/登录（JWT）、`AuthService`、`AuthController`、BCrypt 密码哈希、JWT 配置与角色映射。

- [~] 后端业务API实现
  - 状态: 进行中
  - 说明: 已实现 Users/Partners/Activities 的基础 CRUD 与 Partner-Activity 关联。部分功能（分页/筛选）已实现，仍需完善输入校验、事务和更多业务规则。
  - 相关文件: `backend/src/PartnerManager.Api/Controllers/`、`Repositories/`、`DTOs/`

- [~] 数据库迁移与种子数据
  - 状态: 进行中
  - 说明: 已加入 EF Tools、迁移脚本 `scripts/ef-migrations.ps1`、以及 `DbInitializer`（会自动应用迁移并创建 admin 用户）。请在本地运行 `dotnet ef migrations add InitialCreate` 并提交迁移文件，或让我代为生成占位迁移。

- [ ] 性能与扩展性设计
  - 状态: 未开始
  - 说明: 后续需针对 1k 并发做 Kestrel、连接池、索引、缓存（Redis）、限流与监控设计和实现。

- [~] 前端骨架（React + LESS）
  - 状态: 进行中
  - 说明: 已使用 Vite 创建前端骨架，并实现 fetch 封装与示例页面（Login/Register/Admin 控制台）。文件位于 `frontend/src`。

- [ ] 前端页面实现
  - 状态: 未开始
  - 说明: 管理面板和 CRUD 页面有基础实现，仍需完善表单验证、UX 细节与错误处理。

- [ ] 测试与质量保证
  - 状态: 未开始
  - 说明: 需要补充单元测试、集成测试与压力测试脚本（建议使用 xUnit + EF InMemory/Testcontainer；压力测试使用 k6/JMeter）。

- [ ] 容器化与部署
  - 状态: 未开始
  - 说明: 已添加占位 Dockerfile；还需 `docker-compose`、生产配置和部署脚本。

- [ ] 文档与交付物
  - 状态: 未开始
  - 说明: 需完善 README、Swagger/OpenAPI 文档、Postman 集合与部署说明。


如果你需要，我可以：
- 生成并提交初始 EF migration 文件到仓库（需要确定迁移草案或在环境中运行），
- 生成 Postman 集合用于快速验收，
- 或继续实现剩余 API 校验与前端细化（你选择下一步）。
