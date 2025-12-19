import React, { useEffect, useState } from 'react'
import { getUsers, deleteUser, updateUserRole } from '../services/api'

export default function UsersPage() {
  const [users, setUsers] = useState([])
  const [error, setError] = useState(null)

  async function load() {
    try {
      const res = await getUsers()
      setUsers(res)
    } catch (e) { setError(e?.error || JSON.stringify(e)) }
  }

  useEffect(() => { load() }, [])

  return (
    <div>
      <h3>Users</h3>
      {error && <div className="error">{error}</div>}
      <table>
        <thead><tr><th>ID</th><th>Name</th><th>Email</th><th>Role</th><th>Active</th><th>Actions</th></tr></thead>
        <tbody>
          {users.map(u => (
            <tr key={u.id}>
              <td>{u.id}</td>
              <td>{u.userName}</td>
              <td>{u.email}</td>
              <td>{u.role}</td>
              <td>{u.isActive ? 'Yes' : 'No'}</td>
              <td>
                <button onClick={async () => { await deleteUser(u.id); load() }}>Delete</button>
                <button onClick={async () => { const newRole = u.role === 'Admin' ? 'User' : 'Admin'; await updateUserRole(u.id, newRole); load() }}>Toggle Role</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
