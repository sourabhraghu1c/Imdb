<template>
  <div class="container-fluid px-4 py-4">
    <!-- Header -->
    <div class="d-flex align-items-center mb-4 gap-3">
      <button class="btn btn-outline-secondary btn-sm fw-semibold" @click="$router.push('/movies/add')">
        ADD MOVIE
      </button>
      <div class="ms-auto d-flex gap-2 align-items-center">
        <input
          v-model="yearFilter"
          type="number"
          class="form-control form-control-sm"
          placeholder="Filter by year..."
          style="width:160px"
          @keyup.enter="loadMovies"
        />
        <button class="btn btn-sm btn-outline-primary" @click="loadMovies">
          <i class="bi bi-search"></i>
        </button>
        <button v-if="yearFilter" class="btn btn-sm btn-outline-secondary" @click="clearFilter">
          <i class="bi bi-x"></i>
        </button>
      </div>
    </div>

    <!-- Error -->
    <div v-if="error" class="alert alert-warning py-2 small">{{ error }}</div>

    <!-- Loading -->
    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border text-secondary"></div>
    </div>

    <!-- Grid -->
    <div v-else class="row g-3">
      <div
        v-for="movie in movies"
        :key="movie.id"
        class="col-12 col-sm-6 col-md-4 col-lg-3"
      >
        <div class="movie-card card h-100 border-0 shadow-sm">
          <!-- Poster -->
          <div class="poster-wrap">
            <img
              :src="movie.coverImage || '/no-poster.png'"
              :alt="movie.name"
              class="poster-img"
              @error="e => e.target.src = 'https://placehold.co/300x180?text=No+Poster'"
            />
          </div>

          <div class="card-body d-flex flex-column px-3 pt-2 pb-2">
            <h6 class="card-title fw-bold mb-1 text-uppercase" style="font-size:0.82rem; letter-spacing:0.5px">
              {{ movie.name }}
            </h6>
            <p class="card-text text-muted mb-2" style="font-size:0.78rem; line-height:1.4; max-height:3.2em; overflow:hidden">
              {{ movie.plot || 'No description available.' }}
            </p>

            <!-- Genres -->
            <div v-if="movie.genres?.length" class="d-flex flex-wrap gap-1 mb-2">
              <span v-for="g in movie.genres.slice(0,3)" :key="g.id" class="badge-genre">{{ g.name }}</span>
            </div>

            <!-- Footer -->
            <div class="d-flex align-items-center mt-auto">
              <button
                class="btn btn-link btn-sm p-0 text-decoration-none text-danger fw-semibold"
                style="font-size:0.78rem"
                @click="$router.push(`/movies/${movie.id}`)"
              >
                Explore &nbsp;→
              </button>
              <div class="ms-auto d-flex gap-2">
                <button class="icon-btn" title="Edit" @click="$router.push(`/movies/${movie.id}/edit`)">
                  <i class="bi bi-pencil text-primary" style="font-size:0.8rem"></i>
                </button>
                <button class="icon-btn" title="Delete" @click="confirmDelete(movie)">
                  <i class="bi bi-trash text-danger" style="font-size:0.8rem"></i>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Delete Modal -->
    <div v-if="deleteTarget" class="modal-backdrop-custom" @click.self="deleteTarget=null">
      <div class="modal-box">
        <h6 class="fw-bold mb-2">Delete Movie</h6>
        <p class="text-muted small mb-3">Are you sure you want to delete <strong>{{ deleteTarget.name }}</strong>?</p>
        <div class="d-flex gap-2 justify-content-end">
          <button class="btn btn-sm btn-outline-secondary" @click="deleteTarget=null">Cancel</button>
          <button class="btn btn-sm btn-danger" :disabled="deleting" @click="deleteMovie">
            <span v-if="deleting" class="spinner-border spinner-border-sm me-1"></span>
            Delete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { moviesAPI } from '@/services/api'

const movies = ref([])
const loading = ref(false)
const error = ref('')
const yearFilter = ref('')
const deleteTarget = ref(null)
const deleting = ref(false)

async function loadMovies() {
  loading.value = true
  error.value = ''
  try {
    const res = await moviesAPI.getAll(yearFilter.value || null)
    movies.value = res.data.data
  } catch (e) {
    error.value = e.response?.data?.message || 'Failed to load movies'
    movies.value = []
  } finally {
    loading.value = false
  }
}

function clearFilter() {
  yearFilter.value = ''
  loadMovies()
}

function confirmDelete(movie) {
  deleteTarget.value = movie
}

async function deleteMovie() {
  deleting.value = true
  try {
    await moviesAPI.delete(deleteTarget.value.id)
    movies.value = movies.value.filter(m => m.id !== deleteTarget.value.id)
    deleteTarget.value = null
  } catch (e) {
    alert(e.response?.data?.message || 'Delete failed')
  } finally {
    deleting.value = false
  }
}

onMounted(loadMovies)
</script>

<style scoped>
.movie-card {
  border-radius: 10px;
  transition: transform 0.2s, box-shadow 0.2s;
  cursor: default;
}
.movie-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 20px rgba(0,0,0,0.12) !important;
}
.poster-wrap {
  width: 100%;
  height: 175px;
  overflow: hidden;
  border-radius: 10px 10px 0 0;
  background: #eee;
}
.poster-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
.icon-btn {
  background: none;
  border: none;
  padding: 2px 5px;
  cursor: pointer;
  border-radius: 4px;
}
.icon-btn:hover { background: #f0f0f0; }

/* Delete modal */
.modal-backdrop-custom {
  position: fixed; inset: 0;
  background: rgba(0,0,0,0.4);
  display: flex; align-items: center; justify-content: center;
  z-index: 9999;
}
.modal-box {
  background: #fff;
  border-radius: 10px;
  padding: 24px 28px;
  width: 360px;
  box-shadow: 0 8px 30px rgba(0,0,0,0.2);
}
</style>
