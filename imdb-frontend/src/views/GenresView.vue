<template>
  <div class="container py-4" style="max-width:700px">
    <div class="d-flex align-items-center mb-4">
      <h5 class="fw-bold mb-0">Genres</h5>
      <button class="btn btn-outline-secondary btn-sm ms-4 fw-semibold" @click="openAdd">ADD GENRE</button>
    </div>

    <div v-if="loading" class="text-center py-5"><div class="spinner-border text-secondary"></div></div>
    <div v-else-if="error" class="alert alert-warning">{{ error }}</div>

    <div v-else class="d-flex flex-column gap-2">
      <div v-for="genre in genres" :key="genre.id"
        class="d-flex align-items-center justify-content-between bg-white rounded shadow-sm px-4 py-3">
        <div class="d-flex align-items-center gap-3">
          <span class="genre-dot"></span>
          <span class="fw-semibold">{{ genre.name }}</span>
        </div>
        <div class="d-flex gap-2">
          <button class="icon-btn" @click="openEdit(genre)">
            <i class="bi bi-pencil text-primary small"></i>
          </button>
          <button class="icon-btn" @click="confirmDelete(genre)">
            <i class="bi bi-trash text-danger small"></i>
          </button>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <div v-if="showModal" class="modal-backdrop-custom" @click.self="showModal=false">
      <div class="modal-box">
        <div class="d-flex justify-content-between align-items-center mb-3">
          <h6 class="fw-bold mb-0">{{ editTarget ? 'Edit Genre' : 'Add Genre' }}</h6>
          <button class="btn-close" @click="showModal=false"></button>
        </div>
        <div v-if="formError" class="alert alert-danger small py-2">{{ formError }}</div>
        <div class="mb-3">
          <label class="form-label small fw-semibold">Genre Name</label>
          <input v-model="formName" type="text" class="form-control bg-light border-0" placeholder="e.g. Drama" />
        </div>
        <div class="d-flex gap-2 justify-content-end">
          <button class="btn btn-sm btn-outline-secondary" @click="showModal=false">Cancel</button>
          <button class="btn btn-sm btn-primary" :disabled="saving" @click="save">
            <span v-if="saving" class="spinner-border spinner-border-sm me-1"></span>
            {{ editTarget ? 'Update' : 'Add' }}
          </button>
        </div>
      </div>
    </div>

    <!-- Delete Confirm -->
    <div v-if="deleteTarget" class="modal-backdrop-custom" @click.self="deleteTarget=null">
      <div class="modal-box">
        <h6 class="fw-bold mb-2">Delete Genre</h6>
        <p class="text-muted small mb-3">Delete <strong>{{ deleteTarget.name }}</strong>?</p>
        <div class="d-flex gap-2 justify-content-end">
          <button class="btn btn-sm btn-outline-secondary" @click="deleteTarget=null">Cancel</button>
          <button class="btn btn-sm btn-danger" :disabled="deleting" @click="doDelete">
            <span v-if="deleting" class="spinner-border spinner-border-sm me-1"></span>Delete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { genresAPI } from '@/services/api'

const genres = ref([])
const loading = ref(false)
const error = ref('')
const showModal = ref(false)
const editTarget = ref(null)
const deleteTarget = ref(null)
const formName = ref('')
const formError = ref('')
const saving = ref(false)
const deleting = ref(false)

async function load() {
  loading.value = true; error.value = ''
  try {
    const res = await genresAPI.getAll()
    genres.value = res.data.data
  } catch (e) {
    error.value = e.response?.data?.message || 'Failed to load'; genres.value = []
  } finally { loading.value = false }
}

function openAdd() { editTarget.value = null; formName.value = ''; formError.value = ''; showModal.value = true }
function openEdit(g) { editTarget.value = g; formName.value = g.name; formError.value = ''; showModal.value = true }
function confirmDelete(g) { deleteTarget.value = g }

async function save() {
  if (!formName.value.trim()) { formError.value = 'Name is required'; return }
  saving.value = true
  try {
    if (editTarget.value) await genresAPI.update(editTarget.value.id, { name: formName.value })
    else await genresAPI.create({ name: formName.value })
    showModal.value = false
    await load()
  } catch (e) { formError.value = e.response?.data?.message || 'Save failed' }
  finally { saving.value = false }
}

async function doDelete() {
  deleting.value = true
  try {
    await genresAPI.delete(deleteTarget.value.id)
    genres.value = genres.value.filter(g => g.id !== deleteTarget.value.id)
    deleteTarget.value = null
  } catch (e) { alert(e.response?.data?.message || 'Delete failed') }
  finally { deleting.value = false }
}

onMounted(load)
</script>

<style scoped>
.genre-dot {
  width: 10px; height: 10px; border-radius: 50%;
  background: #f5c518; display: inline-block;
}
.icon-btn { background: none; border: none; padding: 4px 7px; cursor: pointer; border-radius: 4px; }
.icon-btn:hover { background: #f0f0f0; }
.modal-backdrop-custom {
  position: fixed; inset: 0; background: rgba(0,0,0,0.4);
  display: flex; align-items: center; justify-content: center; z-index: 9999;
}
.modal-box {
  background: #fff; border-radius: 10px; padding: 24px 28px;
  width: 380px; box-shadow: 0 8px 30px rgba(0,0,0,0.2);
}
</style>
