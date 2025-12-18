import React, { useState } from 'react'
import { register } from '../services/api'

export default function Register({ onRegister }) {
  const [userName, setUserName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState(null)

  async function handleSubmit(e) {
    e.preventDefault()
    setError(null)
    try {
      await register({ userName, email, password })
      onRegister?.()
    } catch (err) {
      setError(err?.error || JSON.stringify(err))
    }
  }

  return (
    <div className="page auth">
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <label>Username</label>
        <input value={userName} onChange={e => setUserName(e.target.value)} />
        <label>Email</label>
        <input value={email} onChange={e => setEmail(e.target.value)} />
        <label>Password</label>
        <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
        <button type="submit">Register</button>
        {error && <div className="error">{error}</div>}
      </form>
    </div>
  )
}
