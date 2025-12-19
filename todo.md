# 项目 TODO 列表（状态快照）

更新时间: 2025-12-19

- [x] 初始化项目及文档
  - 状态: 完成
  - 说明: 已建立仓库根目录、`cpplan.md`、`README.md`、`.gitignore`，并添加 backend/frontend 说明文件和占位 `Dockerfile`。

- [x] 设计数据模型（Code-First）
  - 状态: 完成
  - 说明: 已实现 `User`、`Partner`、`Activity`、`PartnerActivity` 实体与 `AppDbContext`（文件路径: `backend/src/PartnerManager.Api/Models` 与 `backend/src/PartnerManager.Infrastructure/Data/AppDbContext.cs`）。

- [x] 后端骨架（C# ASP.NET MVC）
  - 状态: 完成
  - 说明: 已创建项目文件、`Program.cs`、依赖注入、EF Core 与 Npgsql 配置、Swagger 与 CORS 支持。

- [x] 实现认证与授权
  - 状态: 完成
  - 说明: 已实现注册/登录（JWT）、`AuthService`、`AuthController`、BCrypt 密码哈希、JWT 配置与角色映射。

- [x] 后端业务API实现
  - 状态: 已完成（基础功能）
  - 说明: Users/Partners/Activities 的 CRUD 与 Partner-Activity 关联已实现；分页/筛选/排序 已移至后端（见 `PartnersController` 与 `PartnerRepository` 实现）。仍有小量业务规则/更严格输入校验可进一步增强，但基础 API 已可用。
  - 相关文件: `backend/src/PartnerManager.Api/Controllers/PartnersController.cs`、`backend/src/PartnerManager.Infrastructure/Repositories/PartnerRepository.cs`、`backend/src/PartnerManager.Api/DTOs/`

- [x] 数据库迁移与种子数据
  - 状态: 完成（迁移已生成并包含新字段）
  - 说明: EF Migrations 已生成并提交（见 `backend/src/PartnerManager.Infrastructure/Migrations/`，包括 `InitialCreate` 与后续 `AddPartnerFields`），`DbInitializer` 存在并会在启动时应用迁移并创建 admin 用户。

- [ ] 性能与扩展性设计
  - 状态: 未开始
  - 说明: 后续需针对 1k 并发做 Kestrel、连接池、索引、缓存（Redis）、限流与监控设计和实现。

- [x] 前端骨架（React + LESS）
  - 状态: 完成
  - 说明: Vite 前端骨架、路由、全局样式（LESS）與 API 封装已实现；已加入 `react-router-dom`、全局加载事件与主题样式（teal）。文件位于 `frontend/src`。

- [~] 前端页面实现
  - 状态: 进行中
  - 说明: `Partners` 的 create/edit/view（右侧抽屉）、验证、服务端分页/筛选/排序、debounce 搜索与全局加载遮罩已实现；其余页面（部分 UX 细节、错误展示、更多表单验证）仍可继续完善。
  - 相关文件：`frontend/src/pages/PartnersPage.jsx`、`frontend/src/services/api.js`、`frontend/src/styles/main.less`。

- [ ] 测试与质量保证
  - 状态: 未开始
  - 说明: 仍需补充单元/集成/压力测试。

- [ ] 容器化与部署
  - 状态: 未开始
  - 说明: 保留占位 `Dockerfile`，需要补充 `docker-compose`、生产配置与 CI/CD 部署脚本。

- [~] 文档与交付物
  - 状态: 进行中
  - 说明: 基本 `README.md` 与后端 Swagger（在程序内可访问）已存在；还需完善使用说明、Postman 集合与部署文档。

如果你需要，我可以：
- 生成并提交额外迁移或在本地运行并确认（当前已有迁移文件）。
- 生成 Postman 集合用于快速验收。
- 在远程仓库创建 release/tag 或继续实现剩余 API 校验与前端细化（你选择下一步）。

已记录的变更说明：
- 已将本地改动提交至本地 Git（首次 commit），并已将仓库强制推送覆盖远程 `https://github.com/AZTest1225/copilottest.git`（master）。
- 已在 `frontend/src/pages/PartnersPage.jsx` 中加入请求并发保护（in-flight guard）并改用 `debouncedSearch` 以减少重复请求；调试日志也已加入以便定位问题。
