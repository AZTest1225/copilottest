import React, { useState } from 'react'
import Login from './pages/Login'
import Register from './pages/Register'
import AdminDashboard from './pages/AdminDashboard'

export default function App() {
  const [view, setView] = useState('login')
  const token = localStorage.getItem('token')

  return (
    <div className="app">
      <header>
        <h1>Partner Manager</h1>
        <nav>
          {!token && <button onClick={() => setView('login')}>Login</button>}
          {!token && <button onClick={() => setView('register')}>Register</button>}
          {token && <button onClick={() => { localStorage.removeItem('token'); window.location.reload() }}>Logout</button>}
          {token && <button onClick={() => setView('admin')}>Admin</button>}
        </nav>
      </header>
      <main>
        {view === 'login' && <Login onLogin={() => setView('admin')} />}
        {view === 'register' && <Register onRegister={() => setView('login')} />}
        {view === 'admin' && <AdminDashboard />}
      </main>
    </div>
  )
}
