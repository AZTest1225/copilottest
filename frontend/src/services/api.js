const API_BASE = window.__API_BASE__ || '/api'

async function apiFetch(path, options = {}) {
  const token = localStorage.getItem('token')
  const headers = { 'Content-Type': 'application/json', ...(options.headers || {}) }
  if (token) headers['Authorization'] = `Bearer ${token}`
  // Emit a global loading start event
  try {
    window.dispatchEvent(new CustomEvent('api:loading', { detail: { path, state: 'start' } }))
  } catch {}
  const res = await fetch(`${API_BASE}${path}`, { ...options, headers })
  const text = await res.text()
  let data = null
  try { data = text ? JSON.parse(text) : null } catch { data = text }
  if (!res.ok) {
    // Emit end before throwing
    try { window.dispatchEvent(new CustomEvent('api:loading', { detail: { path, state: 'end' } })) } catch {}
    throw data || { error: res.statusText }
  }
  // Emit a global loading end event
  try { window.dispatchEvent(new CustomEvent('api:loading', { detail: { path, state: 'end' } })) } catch {}
  return data
}

export async function register(payload) { return apiFetch('/auth/register', { method: 'POST', body: JSON.stringify(payload) }) }
export async function login(payload) { return apiFetch('/auth/login', { method: 'POST', body: JSON.stringify(payload) }) }

// Partners
// Partners list with server-side filtering/sorting
export async function getPartners(paramsOrSearch = null, page = 1, pageSize = 20, sortBy = null, sortOrder = null, sex = null) {
  let search = null
  let p = 1
  let ps = 20
  let sb = null
  let so = null
  let sx = null
  if (paramsOrSearch && typeof paramsOrSearch === 'object') {
    ({ search = null, page: p = 1, pageSize: ps = 20, sortBy: sb = null, sortOrder: so = null, sex: sx = null } = paramsOrSearch)
  } else {
    search = paramsOrSearch
    p = page
    ps = pageSize
    sb = sortBy
    so = sortOrder
    sx = sex
  }
  const params = []
  if (search) params.push(`search=${encodeURIComponent(search)}`)
  if (sx) params.push(`sex=${encodeURIComponent(sx)}`)
  if (sb) params.push(`sortBy=${encodeURIComponent(sb)}`)
  if (so) params.push(`sortOrder=${encodeURIComponent(so)}`)
  params.push(`page=${p}`)
  params.push(`pageSize=${ps}`)
  return apiFetch(`/partners?${params.join('&')}`)
}
export async function getPartner(id) { return apiFetch(`/partners/${id}`) }
export async function createPartner(payload) { return apiFetch('/partners', { method: 'POST', body: JSON.stringify(payload) }) }
export async function updatePartner(id, payload) { return apiFetch(`/partners/${id}`, { method: 'PUT', body: JSON.stringify(payload) }) }
export async function deletePartner(id) { return apiFetch(`/partners/${id}`, { method: 'DELETE' }) }

// Activities
export async function getActivities(search = null, from = null, to = null, page = 1, pageSize = 20) { 
  const params = []
  if (search) params.push(`search=${encodeURIComponent(search)}`)
  if (from) params.push(`from=${encodeURIComponent(from)}`)
  if (to) params.push(`to=${encodeURIComponent(to)}`)
  params.push(`page=${page}`)
  params.push(`pageSize=${pageSize}`)
  return apiFetch(`/activities?${params.join('&')}`)
}
export async function createActivity(payload) { return apiFetch('/activities', { method: 'POST', body: JSON.stringify(payload) }) }
export async function updateActivity(id, payload) { return apiFetch(`/activities/${id}`, { method: 'PUT', body: JSON.stringify(payload) }) }
export async function deleteActivity(id) { return apiFetch(`/activities/${id}`, { method: 'DELETE' }) }

// Users (admin)
export async function getUsers(page = 1, pageSize = 20) { return apiFetch(`/users?page=${page}&pageSize=${pageSize}`) }
export async function deleteUser(id) { return apiFetch(`/users/${id}`, { method: 'DELETE' }) }
export async function updateUserRole(id, role) { return apiFetch(`/users/${id}/role`, { method: 'PUT', body: JSON.stringify(role) }) }

// Partner-Activity association
export async function addPartnerToActivity(activityId, partnerId) { return apiFetch(`/activities/${activityId}/partners`, { method: 'POST', body: JSON.stringify(partnerId) }) }
export async function removePartnerFromActivity(activityId, partnerId) { return apiFetch(`/activities/${activityId}/partners/${partnerId}`, { method: 'DELETE' }) }

export default { apiFetch }
