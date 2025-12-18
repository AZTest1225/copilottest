import React, { useState } from 'react'
import { login } from '../services/api'

export default function Login({ onLogin }) {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState(null)

  async function handleSubmit(e) {
    e.preventDefault()
    setError(null)
    try {
      const res = await login({ email, password })
      localStorage.setItem('token', res.token)
      onLogin?.()
    } catch (err) {
      setError(err?.error || JSON.stringify(err))
    }
  }

  return (
    <div className="page auth">
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <label>Email</label>
        <input value={email} onChange={e => setEmail(e.target.value)} />
        <label>Password</label>
        <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
        <button type="submit">Login</button>
        {error && <div className="error">{error}</div>}
      </form>
    </div>
  )
}
