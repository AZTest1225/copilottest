import React, { useEffect, useState } from 'react'
import { getPartners, getPartner, createPartner, updatePartner, deletePartner } from '../services/api'

export default function PartnersPage() {
  const [partners, setPartners] = useState([])
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)
  const [pageSize] = useState(10)
  const [total, setTotal] = useState(0)
  const [error, setError] = useState(null)
  const [loading, setLoading] = useState(false)

  const [mode, setMode] = useState('list') // list | create | edit | view
  const [selectedId, setSelectedId] = useState(null)
  const emptyForm = { name: '', displayName: '', email: '', sex: '', location: '', phoneNumber: '', address: '' }
  const [form, setForm] = useState(emptyForm)
  const [errors, setErrors] = useState({})
  const [sortBy, setSortBy] = useState('name') // name | displayName | email | sex
  const [sortDir, setSortDir] = useState('asc') // asc | desc
  const [sexFilter, setSexFilter] = useState('') // '' | 'Male' | 'Female'

  const inFlightRef = React.useRef(false)

  async function load(searchValue = search) {
    try {
      if (inFlightRef.current) {
        console.log('load() skipped - request already in flight')
        return
      }
      inFlightRef.current = true
      console.log('load() start', { search: searchValue, page, sortBy, sortDir, sexFilter })
      setLoading(true)
      const res = await getPartners({
        search: searchValue,
        page,
        pageSize,
        sortBy,
        sortOrder: sortDir,
        sex: sexFilter || null
      })
      setPartners(res.items)
      setTotal(res.total)
    } catch (e) { setError(e?.error || JSON.stringify(e)) }
    finally { inFlightRef.current = false; setLoading(false); console.log('load() end') }
  }

  // Debounce search to avoid rapid repeated queries
  const [debouncedSearch, setDebouncedSearch] = useState(search)
  useEffect(() => {
    const t = setTimeout(() => setDebouncedSearch(search), 300)
    return () => clearTimeout(t)
  }, [search])

  useEffect(() => { load(debouncedSearch) }, [debouncedSearch, page, sortBy, sortDir, sexFilter])

  function validateForm(data) {
    const errs = {}
    if (!data.name.trim()) errs.name = 'Name is required'
    if (data.name && data.name.trim().length < 2) errs.name = 'Name must be at least 2 characters'
    if (!data.displayName.trim()) errs.displayName = 'Display name is required'
    if (!data.email.trim()) errs.email = 'Email is required'
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.email)) errs.email = 'Invalid email format'
    if (!data.sex) errs.sex = 'Please choose sex'
    if (data.phoneNumber && !/^[+0-9\-\s]{7,20}$/.test(data.phoneNumber)) errs.phoneNumber = 'Invalid phone number'
    if (!data.address.trim()) errs.address = 'Address is required'
    return errs
  }

  async function handleSave(e) {
    e.preventDefault()
    setError(null)
    const trimmed = {
      name: form.name.trim(),
      displayName: form.displayName.trim(),
      email: form.email.trim(),
      sex: form.sex,
      location: form.location.trim(),
      phoneNumber: form.phoneNumber.trim(),
      address: form.address.trim()
    }
    const v = validateForm(trimmed)
    setErrors(v)
    if (Object.keys(v).length > 0) return

    // Map to backend expected fields (PascalCase). Extra fields are ignored by backend.
    const payload = {
      Name: trimmed.name,
      ContactName: trimmed.displayName,
      ContactEmail: trimmed.email,
      ContactPhone: trimmed.phoneNumber,
      Address: trimmed.address,
      Location: trimmed.location,
      Sex: trimmed.sex
    }

    try {
      if (mode === 'edit' && selectedId) {
        await updatePartner(selectedId, payload)
      } else {
        await createPartner(payload)
      }
      setForm(emptyForm)
      setErrors({})
      setSelectedId(null)
      setMode('list')
      load()
    } catch (err) {
      setError(err?.error || JSON.stringify(err))
    }
  }

  async function handleView(id, goMode = 'view') {
    try {
      const p = await getPartner(id)
      setSelectedId(id)
      setForm({
        name: p.name || '',
        displayName: p.contactName || '',
        email: p.contactEmail || '',
        sex: p.sex || '',
        location: p.location || '',
        phoneNumber: p.contactPhone || '',
        address: p.address || ''
      })
      setMode(goMode)
    } catch (err) {
      setError(err?.error || JSON.stringify(err))
    }
  }

  function handleCreateNew() {
    setForm(emptyForm)
    setErrors({})
    setSelectedId(null)
    setMode('create')
  }
  return (
    <div>
      <h3>Partners</h3>
      {error && <div className="error">{error}</div>}

      {mode === 'list' && (
        <>
          <div className="toolbar">
            <button onClick={handleCreateNew}>New Partner</button>
            <input placeholder="Search" value={search} onChange={e => { setSearch(e.target.value); setPage(1); }} />
            <select aria-label="Sex filter" value={sexFilter} onChange={e => { setSexFilter(e.target.value); setPage(1); }} style={{ marginLeft: 8 }}>
              <option value="">All Sex</option>
              <option value="Male">Male</option>
              <option value="Female">Female</option>
            </select>
          </div>

          {loading && (
            <div className="panel" style={{ display:'flex', alignItems:'center', gap:10, marginBottom:12 }}>
              <div className="spinner" aria-hidden="true" />
              <span className="muted">Loading partners...</span>
            </div>
          )}

          {/* Sorted/filtered table view */}
          <table>
            <thead>
              <tr>
                <th>
                  <button className="ghost-btn" onClick={() => { setSortBy('name'); setSortDir(d => (sortBy === 'name' ? (d === 'asc' ? 'desc' : 'asc') : 'asc')); setPage(1); }}>
                    Name {sortBy === 'name' ? (sortDir === 'asc' ? '▲' : '▼') : ''}
                  </button>
                </th>
                <th>
                  <button className="ghost-btn" onClick={() => { setSortBy('displayName'); setSortDir(d => (sortBy === 'displayName' ? (d === 'asc' ? 'desc' : 'asc') : 'asc')); setPage(1); }}>
                    Display Name {sortBy === 'displayName' ? (sortDir === 'asc' ? '▲' : '▼') : ''}
                  </button>
                </th>
                <th>Email</th>
                <th>Sex</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {partners.map(p => (
                  <tr key={p.id}>
                    <td>{p.name}</td>
                    <td>{p.contactName || ''}</td>
                    <td>{p.contactEmail || ''}</td>
                    <td>{p.sex || ''}</td>
                    <td>
                      <button onClick={() => handleView(p.id, 'view')}>View</button>
                      <button onClick={() => handleView(p.id, 'edit')} style={{ marginLeft: 6 }}>Edit</button>
                      <button onClick={async () => { await deletePartner(p.id); load() }} style={{ marginLeft: 6 }}>Delete</button>
                    </td>
                  </tr>
                ))}
            </tbody>
          </table>

          <div className="pagination">
            <button disabled={loading || page<=1} onClick={() => setPage(p => Math.max(1, p-1))}>Previous</button>
            <span style={{ margin:'0 8px' }}>Page {page} / {Math.max(1, Math.ceil(total / pageSize))} ({total})</span>
            <button disabled={loading || page>=Math.ceil(total / pageSize)} onClick={() => setPage(p => p+1)}>Next</button>
          </div>
        </>
      )}

      {(mode === 'create' || mode === 'edit') && (
        <>
          <div
            className={`drawer-overlay ${mode ? 'open' : ''}`}
            onClick={() => { setMode('list'); setErrors({}); setForm(emptyForm); setSelectedId(null); }}
          />
          <div className={`drawer-panel ${mode ? 'open' : ''}`} aria-modal="true" role="dialog">
            <div className="drawer-header">
              <h4 style={{ margin: 0 }}>{mode === 'create' ? 'New Partner' : 'Edit Partner'}</h4>
              <button
                type="button"
                className="drawer-close ghost-btn"
                onClick={() => { setMode('list'); setErrors({}); setForm(emptyForm); setSelectedId(null); }}
              >Close</button>
            </div>
            <div className="drawer-body">
              <form className="stack" onSubmit={handleSave}>
                <label>Name</label>
                <input value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} className={errors.name ? 'error' : ''} />
                {errors.name && <span className="error-message">{errors.name}</span>}

                <label>Display Name</label>
                <input value={form.displayName} onChange={e => setForm({ ...form, displayName: e.target.value })} className={errors.displayName ? 'error' : ''} />

                <label>Email</label>
                <input type="email" value={form.email} onChange={e => setForm({ ...form, email: e.target.value })} className={errors.email ? 'error' : ''} />
                {errors.email && <span className="error-message">{errors.email}</span>}

                <label>Sex</label>
                <select value={form.sex} onChange={e => setForm({ ...form, sex: e.target.value })} className={errors.sex ? 'error' : ''}>
                  <option value="">Select</option>
                  <option value="Male">Male</option>
                  <option value="Female">Female</option>
                </select>
                {errors.sex && <span className="error-message">{errors.sex}</span>}

                <label>Location</label>
                <input value={form.location} onChange={e => setForm({ ...form, location: e.target.value })} />

                <label>Phone Number</label>
                <input value={form.phoneNumber} onChange={e => setForm({ ...form, phoneNumber: e.target.value })} className={errors.phoneNumber ? 'error' : ''} />
                {errors.phoneNumber && <span className="error-message">{errors.phoneNumber}</span>}

                <label>Address</label>
                <textarea value={form.address} onChange={e => setForm({ ...form, address: e.target.value })} className={errors.address ? 'error' : ''} />
                {errors.address && <span className="error-message">{errors.address}</span>}

                <div style={{ display: 'flex', gap: 8 }}>
                  <button type="submit">{mode === 'create' ? 'Create' : 'Update'}</button>
                  <button type="button" className="ghost-btn" onClick={() => { setMode('list'); setErrors({}); setForm(emptyForm); setSelectedId(null); }}>Cancel</button>
                </div>
              </form>
            </div>
          </div>
        </>
      )}

      {mode === 'view' && (
        <div className="panel" style={{ marginTop: 12 }}>
          <h4>Partner Details</h4>
          <p><strong>Name:</strong> {form.name}</p>
          <p><strong>Display Name:</strong> {form.displayName}</p>
          <p><strong>Email:</strong> {form.email}</p>
          <p><strong>Sex:</strong> {form.sex}</p>
          <p><strong>Location:</strong> {form.location}</p>
          <p><strong>Phone:</strong> {form.phoneNumber}</p>
          <p><strong>Address:</strong> {form.address}</p>
          <div style={{ display: 'flex', gap: 8 }}>
            <button onClick={() => handleView(selectedId, 'edit')}>Edit</button>
            <button className="ghost-btn" onClick={() => setMode('list')}>Back</button>
          </div>
        </div>
      )}
    </div>
  )
}
