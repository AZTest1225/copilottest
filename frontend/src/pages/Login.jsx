import React, { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { login } from '../services/api'

export default function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState(null)
  const navigate = useNavigate()

  useEffect(() => {
    const token = localStorage.getItem('token')
    if (token) {
      navigate('/admin/partners', { replace: true })
    }
  }, [])

  async function handleSubmit(e) {
    e.preventDefault()
    setError(null)
    try {
      const res = await login({ email, password })
      localStorage.setItem('token', res.token)
      navigate('/admin/partners', { replace: true })
    } catch (err) {
      setError(err?.error || JSON.stringify(err))
    }
  }

  return (
    <div className="page auth login-page">
      <div className="login-hero">
        <div className="login-hero__overlay">
          <p className="eyebrow">Welcome back</p>
          <h2>Access your dashboard</h2>
          <p className="muted">Manage partners, activities, and users in one place.</p>
        </div>
      </div>
      <div className="login-card">
        <h3>Login</h3>
        <form onSubmit={handleSubmit} className="stack">
          <label>Email</label>
          <input className="email" placeholder="@UserName" value={email} onChange={e => setEmail(e.target.value)} />
          <label>Password</label>
          <input className="password" placeholder="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} />
          <button type="submit">Login</button>
          {error && <div className="error">{error}</div>}
        </form>
      </div>
    </div>
  )
}
