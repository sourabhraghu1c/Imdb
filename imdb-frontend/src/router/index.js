import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/login',    name: 'Login',    component: () => import('@/views/LoginView.vue'),    meta: { guest: true } },
  { path: '/signup',   name: 'Signup',   component: () => import('@/views/SignupView.vue'),   meta: { guest: true } },
  { path: '/',         redirect: '/movies' },
  { path: '/movies',   name: 'Movies',   component: () => import('@/views/MoviesView.vue'),   meta: { auth: true } },
  { path: '/movies/add',   name: 'AddMovie',  component: () => import('@/views/MovieFormView.vue'), meta: { auth: true } },
  { path: '/movies/:id/edit', name: 'EditMovie', component: () => import('@/views/MovieFormView.vue'), meta: { auth: true } },
  { path: '/movies/:id',  name: 'MovieDetail', component: () => import('@/views/MovieDetailView.vue'), meta: { auth: true } },
  { path: '/actors',   name: 'Actors',   component: () => import('@/views/ActorsView.vue'),   meta: { auth: true } },
  { path: '/producers',name: 'Producers',component: () => import('@/views/ProducersView.vue'),meta: { auth: true } },
  { path: '/genres',   name: 'Genres',   component: () => import('@/views/GenresView.vue'),   meta: { auth: true } },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  if (to.meta.auth && !token) return next('/login')
  if (to.meta.guest && token) return next('/movies')
  next()
})

export default router
