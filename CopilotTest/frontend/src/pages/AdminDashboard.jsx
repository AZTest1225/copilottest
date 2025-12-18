import React, { useState } from 'react'
import UsersPage from './UsersPage'
import PartnersPage from './PartnersPage'
import ActivitiesPage from './ActivitiesPage'

export default function AdminDashboard() {
  const [tab, setTab] = useState('users')
  return (
    <div className="page admin">
      <h2>Admin Dashboard</h2>
      <div className="tabs">
        <button onClick={() => setTab('users')}>Users</button>
        <button onClick={() => setTab('partners')}>Partners</button>
        <button onClick={() => setTab('activities')}>Activities</button>
      </div>
      <div className="tab-content">
        {tab === 'users' && <UsersPage />}
        {tab === 'partners' && <PartnersPage />}
        {tab === 'activities' && <ActivitiesPage />}
      </div>
    </div>
  )
}
