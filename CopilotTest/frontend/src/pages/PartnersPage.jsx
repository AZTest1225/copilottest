import React, { useEffect, useState } from 'react'
import { getPartners, createPartner, deletePartner } from '../services/api'

export default function PartnersPage() {
  const [partners, setPartners] = useState([])
  const [name, setName] = useState('')
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)
  const [pageSize] = useState(10)
  const [total, setTotal] = useState(0)
  const [error, setError] = useState(null)

  async function load() {
    try {
      const res = await getPartners(search, page, pageSize)
      setPartners(res.items)
      setTotal(res.total)
    } catch (e) { setError(e?.error || JSON.stringify(e)) }
  }

  useEffect(() => { load() }, [])

  useEffect(() => { load() }, [search, page])

  async function handleCreate(e) {
    e.preventDefault()
    await createPartner({ name })
    setName('')
    load()
  }

  return (
    <div>
      <h3>Partners</h3>
      {error && <div className="error">{error}</div>}
      <form onSubmit={handleCreate}>
        <input value={name} onChange={e => setName(e.target.value)} placeholder="Partner name" />
        <button type="submit">Create</button>
      </form>
      <div style={{ marginTop: 12 }}>
        <input placeholder="Search" value={search} onChange={e => { setSearch(e.target.value); setPage(1); }} />
      </div>
      <ul>
        {partners.map(p => (
          <li key={p.id}>{p.name} <button onClick={async () => { await deletePartner(p.id); load() }}>Delete</button></li>
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
