<template>
  <nav class="navbar navbar-expand-lg bg-white shadow-sm px-4">
    <a class="navbar-brand d-flex align-items-center gap-2" href="#">
      <!-- Colorful S-like logo matching screenshot -->
      <svg width="32" height="32" viewBox="0 0 32 32" fill="none">
        <rect width="14" height="14" rx="3" fill="#e50914"/>
        <rect x="18" width="14" height="14" rx="3" fill="#f5c518"/>
        <rect y="18" width="14" height="14" rx="3" fill="#2196F3"/>
        <rect x="18" y="18" width="14" height="14" rx="3" fill="#4CAF50"/>
      </svg>
    </a>

    <div class="navbar-nav ms-auto d-flex flex-row gap-4 align-items-center">
      <router-link
        class="nav-link fw-semibold"
        :class="{ 'active-nav': $route.path.startsWith('/movies') }"
        to="/movies"
      >Movies</router-link>

      <router-link
        class="nav-link fw-semibold"
        :class="{ 'active-nav': $route.path.startsWith('/actors') }"
        to="/actors"
      >Actors</router-link>

      <router-link
        class="nav-link fw-semibold"
        :class="{ 'active-nav': $route.path.startsWith('/producers') }"
        to="/producers"
      >Producers</router-link>

      <router-link
        class="nav-link fw-semibold"
        :class="{ 'active-nav': $route.path.startsWith('/genres') }"
        to="/genres"
      >Genres</router-link>

      <button class="btn btn-sm btn-outline-danger ms-2" @click="logout">
        <i class="bi bi-box-arrow-right"></i> Logout
      </button>
    </div>
  </nav>
</template>

<script setup>
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/store/auth'

const router = useRouter()
const authStore = useAuthStore()

function logout() {
  authStore.logout()
  router.push('/login')
}
</script>

<style scoped>
.navbar { border-bottom: 1px solid #eee; }
.nav-link { color: #333; font-size: 0.95rem; }
.nav-link:hover { color: #e50914; }
.active-nav {
  color: #1a1a2e !important;
  border-bottom: 2px solid #1a1a2e;
  padding-bottom: 2px;
}
</style>
