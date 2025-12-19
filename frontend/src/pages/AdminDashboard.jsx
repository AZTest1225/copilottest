import React from 'react'
import { NavLink, Outlet } from 'react-router-dom'

export default function AdminDashboard() {
  return (
    <div className="page admin">
      <h2>Admin Dashboard</h2>
      <div className="tabs">
        <NavLink to="/admin/users"><button>Users</button></NavLink>
        <NavLink to="/admin/partners"><button>Partners</button></NavLink>
        <NavLink to="/admin/activities"><button>Activities</button></NavLink>
      </div>
      <div className="tab-content">
        <Outlet />
      </div>
    </div>
  )
}
