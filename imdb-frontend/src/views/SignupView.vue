<template>
  <div class="min-vh-100 d-flex align-items-center justify-content-center bg-light">
    <div class="card shadow-sm p-4" style="width:420px">
      <div class="text-center mb-4">
        <svg width="40" height="40" viewBox="0 0 32 32" fill="none">
          <rect width="14" height="14" rx="3" fill="#e50914"/>
          <rect x="18" width="14" height="14" rx="3" fill="#f5c518"/>
          <rect y="18" width="14" height="14" rx="3" fill="#2196F3"/>
          <rect x="18" y="18" width="14" height="14" rx="3" fill="#4CAF50"/>
        </svg>
        <h5 class="mt-2 fw-bold">Create Account</h5>
      </div>

      <div v-if="error" class="alert alert-danger py-2 small">{{ error }}</div>
      <div v-if="success" class="alert alert-success py-2 small">Account created! <router-link to="/login">Login now</router-link></div>

      <div class="mb-3">
        <label class="form-label small fw-semibold">Name</label>
        <input v-model="form.name" type="text" class="form-control" placeholder="Your name" />
      </div>
      <div class="mb-3">
        <label class="form-label small fw-semibold">Email</label>
        <input v-model="form.email" type="email" class="form-control" placeholder="you@example.com" />
      </div>
      <div class="mb-4">
        <label class="form-label small fw-semibold">Password</label>
        <input v-model="form.password" type="password" class="form-control" placeholder="••••••••" />
      </div>

      <button class="btn w-100 btn-accent" :disabled="loading" @click="signup">
        <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
        Sign Up
      </button>

      <p class="text-center mt-3 small text-muted">
        Already have an account?
        <router-link to="/login" class="text-decoration-none text-danger">Login</router-link>
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useAuthStore } from '@/store/auth'

const authStore = useAuthStore()
const form = ref({ name: '', email: '', password: '' })
const error = ref('')
const success = ref(false)
const loading = ref(false)

async function signup() {
  error.value = ''
  success.value = false
  loading.value = true
  try {
    await authStore.signup(form.value)
    success.value = true
  } catch (e) {
    error.value = e.response?.data?.message || 'Signup failed'
  } finally {
    loading.value = false
  }
}
</script>
