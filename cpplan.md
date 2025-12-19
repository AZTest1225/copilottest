# Partner 管理系统实现计划

## 概要
- 目标：实现一个支持 Admin 与普通用户的 Partner 管理系统，包含 Partner 与 Activity 的管理和关联，前端使用 React + LESS（fetch 调用 API），后端使用 C# ASP.NET MVC，数据库为 PostgreSQL（Code-First）。系统需支持至少 1k 并发用户。

## 里程碑与任务
1. 初始化项目及仓库
   - 创建后端解决方案与项目（ASP.NET MVC）
   - 创建前端项目（React，使用 CRA 或 Vite）
   - 建立 README 与 `cpplan.md`（本文件）

2. 数据模型设计（Code-First）
   - 实体：
     - `User`：Id, UserName, Email, PasswordHash, Role (Admin/User), CreatedAt, IsActive
     - `Partner`：Id, Name, ContactInfo(JSON/分字段), Status, CreatedAt
     - `Activity`：Id, Title, Description, StartAt, EndAt, Status, CreatedAt
     - `PartnerActivity`（多对多关联）：PartnerId, ActivityId, Role/附加信息, CreatedAt
   - 索引：`User.Email` 唯一，`Partner.Name` 可选索引，Activity 查询需按时间建立索引

3. 后端实现（C# ASP.NET MVC）
   - 使用 EF Core 与 Npgsql provider
   - Code-First Migrations 管理 schema
   - 分层结构：Controllers -> Services -> Repositories -> DbContext
   - DTO 与 AutoMapper 用于输入输出映射

4. 认证与授权
   - 使用 JWT Bearer 进行 API 认证（也可选 Cookie）；支持角色（Admin / User）声明
   - 注册/登录接口，密码使用安全哈希（如 BCrypt）
   - 中间件校验授权并保护 admin 路由

5. 后端 API 设计（RESTful）
   - 用户：POST /api/auth/register, POST /api/auth/login, GET /api/users, PUT/DELETE（Admin）
   - Partner：GET/POST/PUT/DELETE /api/partners
   - Activity：GET/POST/PUT/DELETE /api/activities
   - 关联：POST /api/activities/{id}/partners -> 关联 partner，GET /api/partners/{id}/activities
   - 支持分页（limit/offset 或 cursor）、筛选、排序

6. 数据库迁移与种子数据
   - 编写 EF Core Migrations
   - 提供种子脚本（若干用户、partners、activities）便于本地测试

7. 性能与扩展策略（支持 1k 并发）
   - Kestrel 配置优化与连接池（Npgsql 连接池）
   - 数据库：连接池、查询优化、合理索引、分页限制、避免 N+1 查询
   - 缓存：短期数据使用内存缓存或 Redis（静态字典、频繁读取的列表）
   - API 设计：分页、延迟加载、请求限流（全局或按IP/用户）
   - 监控：Prometheus / Grafana 或 Application Insights

8. 前端实现（React + LESS，fetch）
   - 项目结构：pages、components、services（API 封装）、styles（LESS）
   - 编写 fetch 封装：自动附带 JWT、错误处理、重试策略（必要时）
   - Admin 面板：用户管理、partner 管理、activity 管理与关联操作
   - 普通用户界面：查看、自我资料管理、报名/参与活动（如需）

9. 测试
   - 后端：单元测试（服务层）、集成测试（使用 InMemory DB 或 TestContainer + PostgreSQL）
   - 前端：关键组件测试（Jest + React Testing Library）
   - 压力测试：使用 k6 或 JMeter 模拟并发，重点测试登录、列表查询与写操作

10. 容器化与部署
    - 提供 Dockerfile（后端、前端）与 docker-compose（含 PostgreSQL、Redis 可选）
    - 环境变量管理（连接串、JWT 秘钥）
    - 迁移与种子在容器启动时可选执行

11. 文档交付
    - README: 本地运行说明、迁移、环境变量、端口说明
    - API 文档：Swagger/OpenAPI
    - 部署说明与性能调优建议

## 接口与数据模型示例（简要）
- User DTO
  - id, userName, email, role
- Partner DTO
  - id, name, contact
- Activity DTO
  - id, title, description, startAt, endAt

## 开发与时间预估（粗略）
- 第1周：项目初始化、数据模型设计、后端骨架、认证
- 第2周：后端 CRUD 与关联实现、Migrations、基础前端骨架
- 第3周：前端页面完成、集成、单元测试
- 第4周：性能调优、压力测试、容器化与文档
（视团队规模与并行度可调整）

## 交付物
- 后端源码（ASP.NET MVC、EF Core Migrations）
- 前端源码（React + LESS）
- `cpplan.md`（本文件）、README、Swagger API 文档、Docker 配置、测试脚本

---
如果你同意这个计划，我会按步骤开始实现第一阶段：初始化项目与创建骨架，并将 TODO 状态更新为 `in-progress` 并创建初始仓库结构。