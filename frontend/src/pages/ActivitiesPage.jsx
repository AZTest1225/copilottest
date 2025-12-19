import React, { useEffect, useState } from 'react'
import { getActivities, createActivity, deleteActivity, addPartnerToActivity } from '../services/api'

export default function ActivitiesPage() {
  const [activities, setActivities] = useState([])
  const [title, setTitle] = useState('')
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)
  const [pageSize] = useState(10)
  const [total, setTotal] = useState(0)
  const [error, setError] = useState(null)

  async function load() {
    try {
      const res = await getActivities(search, null, null, page, pageSize)
      setActivities(res.items)
      setTotal(res.total)
    } catch (e) { setError(e?.error || JSON.stringify(e)) }
  }

  useEffect(() => { load() }, [search, page])

  async function handleCreate(e) {
    e.preventDefault()
    await createActivity({ title, startAt: new Date().toISOString(), endAt: new Date().toISOString() })
    setTitle('')
    load()
  }

  return (
    <div>
      <h3>Activities</h3>
      {error && <div className="error">{error}</div>}
      <form onSubmit={handleCreate}>
        <input value={title} onChange={e => setTitle(e.target.value)} placeholder="Activity title" />
        <button type="submit">Create</button>
      </form>
      <div style={{ marginTop: 12 }}>
        <input placeholder="Search" value={search} onChange={e => { setSearch(e.target.value); setPage(1); }} />
      </div>
      <ul>
        {activities.map(a => (
          <li key={a.id}>{a.title} <button onClick={async () => { await deleteActivity(a.id); load() }}>Delete</button></li>
        ))}
      </ul>
      <div>
        <button disabled={page<=1} onClick={() => setPage(p => Math.max(1, p-1))}>Previous</button>
        <span style={{ margin:'0 8px' }}>Page {page} / {Math.max(1, Math.ceil(total / pageSize))} ({total})</span>
        <button disabled={page>=Math.ceil(total / pageSize)} onClick={() => setPage(p => p+1)}>Next</button>
      </div>
    </div>
  )
}
