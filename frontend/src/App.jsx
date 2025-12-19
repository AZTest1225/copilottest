import React, { useEffect, useState } from 'react'
import { BrowserRouter, Routes, Route, Navigate, Link } from 'react-router-dom'
import Login from './pages/Login'
import Register from './pages/Register'
import AdminDashboard from './pages/AdminDashboard'
import UsersPage from './pages/UsersPage'
import PartnersPage from './pages/PartnersPage'
import ActivitiesPage from './pages/ActivitiesPage'
import { Outlet } from 'react-router-dom'

export default function App() {
  const token = localStorage.getItem('token')
  const [view] = useState(token ? 'admin' : 'login')
  const [loadingCount, setLoadingCount] = useState(0)

  useEffect(() => {
    function onApiLoading(e) {
      const state = e?.detail?.state
      setLoadingCount(c => {
        if (state === 'start') return c + 1
        if (state === 'end') return Math.max(0, c - 1)
        return c
      })
    }
    window.addEventListener('api:loading', onApiLoading)
    return () => window.removeEventListener('api:loading', onApiLoading)
  }, [])

  function ProtectedRoute({ children }) {
    const authed = !!localStorage.getItem('token')
    return authed ? children : <Navigate to="/login" replace />
  }

  return (
    <BrowserRouter>
      <div className="app">
        <header>
          <h1>Partner Manager</h1>
          <nav>
            {!token && <Link to="/login"><button>Login</button></Link>}
            {!token && <Link to="/register"><button>Register</button></Link>}
            {token && <button onClick={() => { localStorage.removeItem('token'); window.location.href = '/login' }}>Logout</button>}
            {token && <Link to="/admin/partners"><button>Admin</button></Link>}
          </nav>
        </header>
        <main>
          <Routes>
            <Route path="/" element={<Navigate to={token ? '/admin/partners' : '/login'} replace />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />

            <Route path="/admin" element={<ProtectedRoute><AdminDashboard /></ProtectedRoute>}>
              <Route index element={<Navigate to="partners" replace />} />
              <Route path="users" element={<UsersPage />} />
              <Route path="partners" element={<PartnersPage />} />
              <Route path="activities" element={<ActivitiesPage />} />
            </Route>

            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </main>
        {loadingCount > 0 && (
          <div className="app-loading-overlay" aria-live="polite" aria-busy="true">
            <div className="spinner" />
            <div className="loading-text">Loading...</div>
          </div>
        )}
      </div>
    </BrowserRouter>
  )
}
