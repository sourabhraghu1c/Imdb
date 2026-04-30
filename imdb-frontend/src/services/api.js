import axios from 'axios'

const BASE_URL = 'https://localhost:44338/api'

const api = axios.create({
  baseURL: BASE_URL,
  headers: { 'Content-Type': 'application/json' }
})

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

api.interceptors.response.use(
  res => res,
  err => {
    if (err.response?.status === 401) {
      localStorage.removeItem('token')
      window.location.href = '/login'
    }
    return Promise.reject(err)
  }
)

export const authAPI = {
  login:  (data) => api.post('/auth/login', data),
  signup: (data) => api.post('/auth/signup', data)
}

export const moviesAPI = {
  getAll:       (year)      => api.get('/movies', { params: year ? { year } : {} }),
  getById:      (id)        => api.get(`/movies/${id}`),
  create:       (data)      => api.post('/movies', data),
  update:       (id, data)  => api.put(`/movies/${id}`, data),
  uploadPoster: (id, file)  => {
    const form = new FormData()
    form.append('file', file)
    return api.patch(`/movies/${id}/poster`, form, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
  },
  delete: (id) => api.delete(`/movies/${id}`)
}

export const actorsAPI = {
  getAll:  ()           => api.get('/actors'),
  getById: (id)         => api.get(`/actors/${id}`),
  create:  (data)       => api.post('/actors', data),
  update:  (id, data)   => api.put(`/actors/${id}`, data),
  delete:  (id)         => api.delete(`/actors/${id}`)
}

export const producersAPI = {
  getAll:  ()           => api.get('/producers'),
  getById: (id)         => api.get(`/producers/${id}`),
  create:  (data)       => api.post('/producers', data),
  update:  (id, data)   => api.put(`/producers/${id}`, data),
  delete:  (id)         => api.delete(`/producers/${id}`)
}

export const genresAPI = {
  getAll:  ()           => api.get('/genres'),
  getById: (id)         => api.get(`/genres/${id}`),
  create:  (data)       => api.post('/genres', data),
  update:  (id, data)   => api.put(`/genres/${id}`, data),
  delete:  (id)         => api.delete(`/genres/${id}`)
}

export const reviewsAPI = {
  getAll:  (movieId)              => api.get(`/movies/${movieId}/reviews`),
  getById: (movieId, id)          => api.get(`/movies/${movieId}/reviews/${id}`),
  create:  (movieId, data)        => api.post(`/movies/${movieId}/reviews`, data),
  update:  (movieId, id, data)    => api.put(`/movies/${movieId}/reviews/${id}`, data),
  delete:  (movieId, id)          => api.delete(`/movies/${movieId}/reviews/${id}`)
}

export default api
