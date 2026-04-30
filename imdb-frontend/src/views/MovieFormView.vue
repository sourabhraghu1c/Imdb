<template>
  <div class="container py-5" style="max-width:720px">
    <h4 class="fw-bold mb-4">{{ isEdit ? 'Edit Movie' : 'Enter Movie Details' }}</h4>

    <div v-if="pageError" class="alert alert-danger small">{{ pageError }}</div>

    <!-- Movie Name -->
    <div class="mb-4">
      <label class="form-label fw-semibold">Movie Name</label>
      <input v-model="form.name" type="text" class="form-control form-control-lg bg-light border-0"
        placeholder="You know what goes here !" />
      <div v-if="errors.name" class="text-danger small mt-1">{{ errors.name }}</div>
    </div>

    <!-- Year of Release -->
    <div class="mb-4">
      <label class="form-label fw-semibold">Year of Release</label>
      <input v-model.number="form.yearOfRelease" type="number" class="form-control form-control-lg bg-light border-0"
        placeholder="20XX" />
      <div v-if="errors.yearOfRelease" class="text-danger small mt-1">{{ errors.yearOfRelease }}</div>
    </div>

    <!-- Actors -->
    <div class="mb-4">
      <label class="form-label fw-semibold">Actors</label>
      <div class="d-flex gap-2 align-items-center">
        <select v-model="selectedActorId" class="form-select bg-light border-0">
          <option value="">Select from the list</option>
          <option v-for="a in actors" :key="a.id" :value="a.id">{{ a.name }}</option>
        </select>
        <button class="btn btn-outline-secondary btn-sm text-nowrap px-3" @click="addActor">ADD ACTOR</button>
      </div>
      <!-- Selected actors tags -->
      <div class="d-flex flex-wrap gap-2 mt-2">
        <span v-for="id in form.actorIds" :key="id" class="badge-actor d-flex align-items-center gap-1 px-2 py-1">
          {{ actorName(id) }}
          <button class="btn-close btn-close-sm ms-1" style="font-size:0.55rem" @click="removeActor(id)"></button>
        </span>
      </div>
      <div v-if="errors.actorIds" class="text-danger small mt-1">{{ errors.actorIds }}</div>
    </div>

    <!-- Producer -->
    <div class="mb-4">
      <label class="form-label fw-semibold">Producer</label>
      <div class="d-flex gap-2 align-items-center">
        <select v-model="form.producerId" class="form-select bg-light border-0">
          <option :value="0">Select from the list</option>
          <option v-for="p in producers" :key="p.id" :value="p.id">{{ p.name }}</option>
        </select>
        <span class="text-muted small text-nowrap px-3">ADD PRODUCER</span>
      </div>
      <div v-if="errors.producerId" class="text-danger small mt-1">{{ errors.producerId }}</div>
    </div>

    <!-- Genres -->
    <div class="mb-4">
      <label class="form-label fw-semibold">Genres</label>
      <select v-model="selectedGenreId" class="form-select bg-light border-0" @change="addGenre">
        <option value="">Select from the list</option>
        <option v-for="g in genres" :key="g.id" :value="g.id">{{ g.name }}</option>
      </select>
      <div class="d-flex flex-wrap gap-2 mt-2">
        <span v-for="id in form.genreIds" :key="id" class="badge-genre d-flex align-items-center gap-1 px-2 py-1">
          {{ genreName(id) }}
          <button class="btn-close btn-close-sm ms-1" style="font-size:0.55rem" @click="removeGenre(id)"></button>
        </span>
      </div>
      <div v-if="errors.genreIds" class="text-danger small mt-1">{{ errors.genreIds }}</div>
    </div>

    <!-- Plot -->
    <div class="mb-4">
      <label class="form-label fw-semibold">Plot</label>
      <textarea v-model="form.plot" rows="4" class="form-control bg-light border-0"
        placeholder="Describe the beautiful story"></textarea>
      <div v-if="errors.plot" class="text-danger small mt-1">{{ errors.plot }}</div>
    </div>

    <!-- Poster Upload (only for add, or separate patch) -->
    <div v-if="!isEdit" class="mb-4">
      <label class="form-label fw-semibold">Poster</label>
      <input type="file" class="form-control bg-light border-0" accept="image/*" @change="onFileChange" />
      <p class="text-muted small mt-1">Upload after creating the movie via poster upload button.</p>
    </div>

    <!-- Submit -->
    <div class="d-flex gap-3 mt-2">
      <button class="btn btn-primary px-4" :disabled="submitting" @click="submit">
        <span v-if="submitting" class="spinner-border spinner-border-sm me-2"></span>
        {{ isEdit ? 'UPDATE' : 'ADD' }}
      </button>
      <button class="btn btn-outline-secondary px-4" @click="$router.push('/movies')">Cancel</button>
    </div>

    <!-- Poster upload section for edit -->
    <div v-if="isEdit" class="mt-5 pt-4 border-top">
      <h6 class="fw-semibold mb-3">Update Movie Poster</h6>
      <div class="d-flex gap-2 align-items-center">
        <input type="file" class="form-control bg-light border-0" accept="image/*" @change="onFileChange" />
        <button class="btn btn-outline-primary btn-sm text-nowrap" :disabled="uploading || !posterFile" @click="uploadPoster">
          <span v-if="uploading" class="spinner-border spinner-border-sm me-1"></span>
          Upload Poster
        </button>
      </div>
      <div v-if="posterSuccess" class="text-success small mt-2">Poster updated successfully!</div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { moviesAPI, actorsAPI, producersAPI, genresAPI } from '@/services/api'

const route = useRoute()
const router = useRouter()

const isEdit = computed(() => !!route.params.id)

const form = reactive({
  name: '',
  yearOfRelease: '',
  plot: '',
  coverImage: '',
  producerId: 0,
  actorIds: [],
  genreIds: []
})

const errors = reactive({})
const pageError = ref('')
const submitting = ref(false)
const uploading = ref(false)
const posterSuccess = ref(false)
const posterFile = ref(null)

const actors = ref([])
const producers = ref([])
const genres = ref([])

const selectedActorId = ref('')
const selectedGenreId = ref('')

function actorName(id) {
  return actors.value.find(a => a.id === id)?.name || id
}
function genreName(id) {
  return genres.value.find(g => g.id === id)?.name || id
}

function addActor() {
  const id = parseInt(selectedActorId.value)
  if (!id || form.actorIds.includes(id)) return
  form.actorIds.push(id)
  selectedActorId.value = ''
}
function removeActor(id) {
  form.actorIds = form.actorIds.filter(a => a !== id)
}
function addGenre() {
  const id = parseInt(selectedGenreId.value)
  if (!id || form.genreIds.includes(id)) return
  form.genreIds.push(id)
  selectedGenreId.value = ''
}
function removeGenre(id) {
  form.genreIds = form.genreIds.filter(g => g !== id)
}
function onFileChange(e) {
  posterFile.value = e.target.files[0]
}

function validate() {
  Object.keys(errors).forEach(k => delete errors[k])
  let valid = true
  if (!form.name.trim()) { errors.name = 'Movie name is required'; valid = false }
  if (!form.yearOfRelease) { errors.yearOfRelease = 'Year is required'; valid = false }
  if (!form.plot.trim()) { errors.plot = 'Plot is required'; valid = false }
  if (!form.producerId) { errors.producerId = 'Select a producer'; valid = false }
  if (!form.actorIds.length) { errors.actorIds = 'Add at least one actor'; valid = false }
  if (!form.genreIds.length) { errors.genreIds = 'Add at least one genre'; valid = false }
  return valid
}

async function submit() {
  if (!validate()) return
  submitting.value = true
  pageError.value = ''
  try {
    const payload = {
      name: form.name,
      yearOfRelease: form.yearOfRelease,
      plot: form.plot,
      coverImage: form.coverImage || '',
      producerId: form.producerId,
      actorIds: form.actorIds,
      genreIds: form.genreIds
    }
    if (isEdit.value) {
      await moviesAPI.update(route.params.id, payload)
    } else {
      const res = await moviesAPI.create(payload)
      const newId = res.data.data
      // If poster file selected, upload it now
      if (posterFile.value && newId) {
        await moviesAPI.uploadPoster(newId, posterFile.value)
      }
    }
    router.push('/movies')
  } catch (e) {
    pageError.value = e.response?.data?.message || 'Failed to save movie'
  } finally {
    submitting.value = false
  }
}

async function uploadPoster() {
  if (!posterFile.value) return
  uploading.value = true
  posterSuccess.value = false
  try {
    await moviesAPI.uploadPoster(route.params.id, posterFile.value)
    posterSuccess.value = true
  } catch (e) {
    pageError.value = e.response?.data?.message || 'Upload failed'
  } finally {
    uploading.value = false
  }
}

async function loadDependencies() {
  try {
    const [ar, pr, gr] = await Promise.all([
      actorsAPI.getAll(),
      producersAPI.getAll(),
      genresAPI.getAll()
    ])
    actors.value = ar.data.data
    producers.value = pr.data.data
    genres.value = gr.data.data
  } catch (e) {
    pageError.value = 'Failed to load actors/producers/genres. Make sure they exist.'
  }
}

async function loadMovie() {
  try {
    const res = await moviesAPI.getById(route.params.id)
    const m = res.data.data
    form.name = m.name
    form.yearOfRelease = m.yearOfRelease
    form.plot = m.plot
    form.coverImage = m.coverImage || ''
    form.producerId = m.producer?.id || 0
    form.actorIds = m.actors?.map(a => a.id) || []
    form.genreIds = m.genres?.map(g => g.id) || []
  } catch (e) {
    pageError.value = 'Failed to load movie'
  }
}

onMounted(async () => {
  await loadDependencies()
  if (isEdit.value) await loadMovie()
})
</script>
