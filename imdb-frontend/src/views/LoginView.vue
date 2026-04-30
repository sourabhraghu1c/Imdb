<template>
  <div class="min-vh-100 d-flex align-items-center justify-content-center bg-light">
    <div class="card shadow-sm p-4" style="width:400px">
      <div class="text-center mb-4">
        <svg width="40" height="40" viewBox="0 0 32 32" fill="none">
          <rect width="14" height="14" rx="3" fill="#e50914"/>
          <rect x="18" width="14" height="14" rx="3" fill="#f5c518"/>
          <rect y="18" width="14" height="14" rx="3" fill="#2196F3"/>
          <rect x="18" y="18" width="14" height="14" rx="3" fill="#4CAF50"/>
        </svg>
        <h5 class="mt-2 fw-bold">IMDB Sample</h5>
        <p class="text-muted small">Sign in to continue</p>
      </div>

      <div v-if="error" class="alert alert-danger py-2 small">{{ error }}</div>

      <div class="mb-3">
        <label class="form-label small fw-semibold">Email</label>
        <input v-model="form.email" type="email" class="form-control" placeholder="you@example.com" />
      </div>
      <div class="mb-4">
        <label class="form-label small fw-semibold">Password</label>
        <input v-model="form.password" type="password" class="form-control" placeholder="••••••••" />
      </div>

      <button class="btn w-100 btn-accent" :disabled="loading" @click="login">
        <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
        Login
      </button>

      <p class="text-center mt-3 small text-muted">
        No account?
        <router-link to="/signup" class="text-decoration-none text-danger">Sign up</router-link>
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/store/auth'

const router = useRouter()
const authStore = useAuthStore()

const form = ref({ email: '', password: '' })
const error = ref('')
const loading = ref(false)

async function login() {
  error.value = ''
  loading.value = true
  try {
    await authStore.login(form.value)
    router.push('/movies')
  } catch (e) {
    error.value = e.response?.data?.message || 'Invalid credentials'
  } finally {
    loading.value = false
  }
}
</script>
