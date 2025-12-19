import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { register } from '../services/api'

export default function Register() {
  const [userName, setUserName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [errors, setErrors] = useState({})
  const [error, setError] = useState(null)
  const navigate = useNavigate()

  function validateForm() {
    const newErrors = {}

    // Username validation
    if (!userName.trim()) {
      newErrors.userName = 'Username is required'
    } else if (userName.length < 3) {
      newErrors.userName = 'Username must be at least 3 characters'
    } else if (userName.length > 20) {
      newErrors.userName = 'Username must not exceed 20 characters'
    }

    // Email validation
    if (!email.trim()) {
      newErrors.email = 'Email is required'
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      newErrors.email = 'Please enter a valid email address'
    }

    // Password validation
    if (!password) {
      newErrors.password = 'Password is required'
    } else if (password.length < 6) {
      newErrors.password = 'Password must be at least 6 characters'
    } else if (password.length > 50) {
      newErrors.password = 'Password must not exceed 50 characters'
    } else if (!/[A-Z]/.test(password)) {
      newErrors.password = 'Password must contain at least one uppercase letter'
    } else if (!/[a-z]/.test(password)) {
      newErrors.password = 'Password must contain at least one lowercase letter'
    } else if (!/[0-9]/.test(password)) {
      newErrors.password = 'Password must contain at least one number'
    }

    // Confirm password validation
    if (!confirmPassword) {
      newErrors.confirmPassword = 'Please confirm your password'
    } else if (password !== confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match'
    }

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  async function handleSubmit(e) {
    e.preventDefault()
    setError(null)

    if (!validateForm()) {
      return
    }

    try {
      await register({ userName, email, password })
      navigate('/login', { replace: true })
    } catch (err) {
      setError(err?.error || JSON.stringify(err))
    }
  }

  return (
    <div className="page auth register-page">
      <div className="register-hero">
        <div className="register-hero__overlay">
          <p className="eyebrow">Join us</p>
          <h2>Create your account</h2>
          <p className="muted">Start managing your partners and activities today.</p>
        </div>
      </div>
      <div className="register-card">
        <h3>Register</h3>
        <form onSubmit={handleSubmit} className="stack">
          <div className="form-group">
            <label>Username</label>
            <input 
              value={userName} 
              onChange={e => setUserName(e.target.value)}
              className={errors.userName ? 'error' : ''}
              placeholder="Choose a username (3-20 characters)"
            />
            {errors.userName && <span className="error-message">{errors.userName}</span>}
          </div>

          <div className="form-group">
            <label>Email</label>
            <input 
              type="email"
              value={email} 
              onChange={e => setEmail(e.target.value)}
              className={errors.email ? 'error' : ''}
              placeholder="your@email.com"
            />
            {errors.email && <span className="error-message">{errors.email}</span>}
          </div>

          <div className="form-group">
            <label>Password</label>
            <input 
              type="password" 
              value={password} 
              onChange={e => setPassword(e.target.value)}
              className={errors.password ? 'error' : ''}
              placeholder="At least 6 characters with uppercase, lowercase, and number"
            />
            {errors.password && <span className="error-message">{errors.password}</span>}
          </div>

          <div className="form-group">
            <label>Confirm Password</label>
            <input 
              type="password" 
              value={confirmPassword} 
              onChange={e => setConfirmPassword(e.target.value)}
              className={errors.confirmPassword ? 'error' : ''}
              placeholder="Re-enter your password"
            />
            {errors.confirmPassword && <span className="error-message">{errors.confirmPassword}</span>}
          </div>

          <button type="submit">Register</button>
          {error && <div className="error">{error}</div>}
        </form>
      </div>
    </div>
  )
}
