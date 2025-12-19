# 前端（React + LESS）

说明：
本目录用于前端代码（建议使用 Vite 或 CRA）。样式使用 LESS，前端通过 `fetch` 调用后端 API（携带 JWT）。

建议初始化（Vite + React）：

```bash
cd frontend
npm create vite@latest . -- --template react
npm install
# 安装 less
npm install --save-dev less
```

项目约定：
- `src/pages`：页面
- `src/components`：可复用组件
- `src/services/api.js`：fetch 封装（统一处理 JWT）
- `src/styles`：LESS 文件

示例 `fetch` 封装（简要）

```js
// src/services/api.js
export async function apiFetch(path, options = {}) {
  const token = localStorage.getItem('token')
  const headers = { 'Content-Type': 'application/json', ...(options.headers || {}) }
  if (token) headers['Authorization'] = `Bearer ${token}`
  const res = await fetch(`/api/${path}`, { ...options, headers })
  if (!res.ok) throw new Error(await res.text())
  return res.json()
}
```

下一步我将搭建 `src` 目录与基础页面（如你同意）。