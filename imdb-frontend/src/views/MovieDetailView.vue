<template>
  <div class="container py-5" style="max-width:800px">
    <button class="btn btn-sm btn-outline-secondary mb-4" @click="$router.push('/movies')">
      <i class="bi bi-arrow-left"></i> Back to Movies
    </button>

    <div v-if="loading" class="text-center py-5"><div class="spinner-border"></div></div>

    <div v-else-if="movie">
      <div class="row g-4 mb-5">
        <!-- Poster -->
        <div class="col-md-4">
          <img
            :src="movie.coverImage || 'https://placehold.co/300x400?text=No+Poster'"
            class="img-fluid rounded shadow"
            :alt="movie.name"
            style="width:100%; max-height:380px; object-fit:cover"
          />
        </div>
        <!-- Info -->
        <div class="col-md-8">
          <h3 class="fw-bold mb-1">{{ movie.name }}</h3>
          <p class="text-muted mb-3">{{ movie.yearOfRelease }}</p>

          <div class="mb-3">
            <span class="fw-semibold small text-uppercase text-muted">Producer</span><br/>
            <span class="badge bg-secondary mt-1">{{ movie.producer?.name }}</span>
          </div>

          <div class="mb-3">
            <span class="fw-semibold small text-uppercase text-muted">Actors</span>
            <div class="d-flex flex-wrap gap-1 mt-1">
              <span v-for="a in movie.actors" :key="a.id" class="badge-actor">{{ a.name }}</span>
            </div>
          </div>

          <div class="mb-3">
            <span class="fw-semibold small text-uppercase text-muted">Genres</span>
            <div class="d-flex flex-wrap gap-1 mt-1">
              <span v-for="g in movie.genres" :key="g.id" class="badge-genre">{{ g.name }}</span>
            </div>
          </div>

          <div>
            <span class="fw-semibold small text-uppercase text-muted">Plot</span>
            <p class="mt-1" style="line-height:1.7">{{ movie.plot }}</p>
          </div>

          <button class="btn btn-primary btn-sm mt-2" @click="$router.push(`/movies/${movie.id}/edit`)">
            <i class="bi bi-pencil me-1"></i> Edit Movie
          </button>
        </div>
      </div>

      <!-- Reviews Section -->
      <div class="border-top pt-4">
        <h5 class="fw-bold mb-3">Reviews</h5>

        <!-- Add Review -->
        <div class="d-flex gap-2 mb-4">
          <input
            v-model="newReview"
            type="text"
            class="form-control bg-light border-0"
            placeholder="Write a review..."
            @keyup.enter="addReview"
          />
          <button class="btn btn-accent text-nowrap" :disabled="addingReview" @click="addReview">
            <span v-if="addingReview" class="spinner-border spinner-border-sm me-1"></span>
            Add Review
          </button>
        </div>

        <div v-if="reviewsLoading" class="text-center py-3"><div class="spinner-border spinner-border-sm"></div></div>

        <div v-else-if="!reviews.length" class="text-muted small fst-italic">No reviews yet. Be the first!</div>

        <div v-else class="d-flex flex-column gap-2">
          <div v-for="r in reviews" :key="r.id" class="review-item p-3 rounded bg-white shadow-sm d-flex justify-content-between align-items-start">
            <div>
              <i class="bi bi-chat-quote-fill text-muted me-2 small"></i>
              <span v-if="editingReview?.id !== r.id" style="font-size:0.9rem">{{ r.message }}</span>
              <input
                v-else
                v-model="editingReview.message"
                class="form-control form-control-sm d-inline-block"
                style="width:auto; min-width:200px"
              />
            </div>
            <div class="d-flex gap-2 ms-3">
              <template v-if="editingReview?.id !== r.id">
                <button class="icon-btn" @click="startEdit(r)">
                  <i class="bi bi-pencil text-primary small"></i>
                </button>
                <button class="icon-btn" @click="deleteReview(r.id)">
                  <i class="bi bi-trash text-danger small"></i>
                </button>
              </template>
              <template v-else>
                <button class="btn btn-sm btn-primary py-0 px-2" @click="saveEdit">Save</button>
                <button class="btn btn-sm btn-outline-secondary py-0 px-2" @click="editingReview=null">Cancel</button>
              </template>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { moviesAPI, reviewsAPI } from '@/services/api'

const route = useRoute()
const movieId = parseInt(route.params.id)

const movie = ref(null)
const loading = ref(false)
const reviews = ref([])
const reviewsLoading = ref(false)
const newReview = ref('')
const addingReview = ref(false)
const editingReview = ref(null)

async function loadMovie() {
  loading.value = true
  try {
    const res = await moviesAPI.getById(movieId)
    movie.value = res.data.data
  } finally {
    loading.value = false
  }
}

async function loadReviews() {
  reviewsLoading.value = true
  try {
    const res = await reviewsAPI.getAll(movieId)
    reviews.value = res.data.data
  } catch {
    reviews.value = []
  } finally {
    reviewsLoading.value = false
  }
}

async function addReview() {
  if (!newReview.value.trim()) return
  addingReview.value = true
  try {
    await reviewsAPI.create(movieId, { message: newReview.value.trim() })
    newReview.value = ''
    await loadReviews()
  } finally {
    addingReview.value = false
  }
}

function startEdit(r) {
  editingReview.value = { id: r.id, message: r.message }
}

async function saveEdit() {
  try {
    await reviewsAPI.update(movieId, editingReview.value.id, { message: editingReview.value.message })
    editingReview.value = null
    await loadReviews()
  } catch (e) {
    alert(e.response?.data?.message || 'Update failed')
  }
}

async function deleteReview(id) {
  try {
    await reviewsAPI.delete(movieId, id)
    reviews.value = reviews.value.filter(r => r.id !== id)
  } catch (e) {
    alert(e.response?.data?.message || 'Delete failed')
  }
}

onMounted(() => {
  loadMovie()
  loadReviews()
})
</script>

<style scoped>
.review-item { border: 1px solid #f0f0f0; }
.icon-btn { background: none; border: none; padding: 2px 5px; cursor: pointer; border-radius: 4px; }
.icon-btn:hover { background: #f5f5f5; }
</style>
