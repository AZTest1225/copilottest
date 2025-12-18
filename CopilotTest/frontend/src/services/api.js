const API_BASE = window.__API_BASE__ || '/api'

async function apiFetch(path, options = {}) {
  const token = localStorage.getItem('token')
  const headers = { 'Content-Type': 'application/json', ...(options.headers || {}) }
  if (token) headers['Authorization'] = `Bearer ${token}`
  const res = await fetch(`${API_BASE}${path}`, { ...options, headers })
  const text = await res.text()
  let data = null
  try { data = text ? JSON.parse(text) : null } catch { data = text }
  if (!res.ok) throw data || { error: res.statusText }
  return data
}

export async function register(payload) { return apiFetch('/auth/register', { method: 'POST', body: JSON.stringify(payload) }) }
export async function login(payload) { return apiFetch('/auth/login', { method: 'POST', body: JSON.stringify(payload) }) }

// Partners
export async function getPartners(search = null, page = 1, pageSize = 20) { return apiFetch(`/partners?${search ? `search=${encodeURIComponent(search)}&` : ''}page=${page}&pageSize=${pageSize}`) }
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
