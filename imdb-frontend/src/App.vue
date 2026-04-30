<template>
  <div>
    <NavBar v-if="authStore.isAuthenticated" />
    <router-view />
  </div>
</template>

<script setup>
import NavBar from '@/components/NavBar.vue'
import { useAuthStore } from '@/store/auth'
const authStore = useAuthStore()
</script>

<style>
:root {
  --primary: #1a1a2e;
  --accent: #e50914;
  --accent-soft: #f5c518;
  --surface: #f8f9fa;
  --card-shadow: 0 2px 12px rgba(0,0,0,0.08);
}

body {
  background: var(--surface);
  font-family: 'Segoe UI', sans-serif;
}

.btn-accent {
  background: var(--accent);
  color: #fff;
  border: none;
}
.btn-accent:hover { background: #c40812; color: #fff; }

.text-accent { color: var(--accent); }
.badge-genre {
  background: #fff3cd;
  color: #856404;
  border: 1px solid #ffc107;
  font-size: 0.72rem;
  padding: 2px 8px;
  border-radius: 20px;
}
.badge-actor {
  background: #cfe2ff;
  color: #084298;
  border: 1px solid #b6d4fe;
  font-size: 0.72rem;
  padding: 2px 8px;
  border-radius: 20px;
}
</style>
