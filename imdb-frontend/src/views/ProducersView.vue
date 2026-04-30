<template>
  <div class="container-fluid px-4 py-4">
    <div class="d-flex align-items-center mb-4">
      <h5 class="fw-bold mb-0">Producers</h5>
      <button class="btn btn-outline-secondary btn-sm ms-4 fw-semibold" @click="openAdd">ADD PRODUCER</button>
    </div>

    <div v-if="loading" class="text-center py-5"><div class="spinner-border text-secondary"></div></div>
    <div v-else-if="error" class="alert alert-warning">{{ error }}</div>

    <div v-else class="row g-3">
      <div v-for="producer in producers" :key="producer.id" class="col-12 col-sm-6 col-md-4 col-lg-3">
        <div class="card border-0 shadow-sm h-100 p-3">
          <div class="d-flex align-items-center gap-3 mb-2">
            <div class="avatar-circle" style="background:#0d6efd">{{ producer.name.charAt(0) }}</div>
            <div>
              <div class="fw-bold" style="font-size:0.9rem">{{ producer.name }}</div>
              <div class="text-muted small">{{ producer.gender === 'M' ? 'Male' : 'Female' }} · {{ producer.dob?.substring(0,4) }}</div>
            </div>
          </div>
          <p class="text-muted small mb-3" style="line-height:1.5; max-height:3em; overflow:hidden">{{ producer.bio }}</p>
          <div class="d-flex gap-2 mt-auto">
            <button class="btn btn-sm btn-outline-primary flex-fill" @click="openEdit(producer)">
              <i class="bi bi-pencil me-1"></i> Edit
            </button>
            <button class="btn btn-sm btn-outline-danger flex-fill" @click="confirmDelete(producer)">
              <i class="bi bi-trash me-1"></i> Delete
            </button>
          </div>
        </div>
      </div>
    </div>

    <PersonModal
      v-if="showModal"
      title="Producer"
      :editData="editTarget"
      @close="showModal=false"
      @saved="handleSave"
    />

    <div v-if="deleteTarget" class="modal-backdrop-custom" @click.self="deleteTarget=null">
      <div class="modal-box">
        <h6 class="fw-bold mb-2">Delete Producer</h6>
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
import { producersAPI } from '@/services/api'
import PersonModal from '@/components/PersonModal.vue'

const producers = ref([])
const loading = ref(false)
const error = ref('')
const showModal = ref(false)
const editTarget = ref(null)
const deleteTarget = ref(null)
const deleting = ref(false)

async function load() {
  loading.value = true; error.value = ''
  try {
    const res = await producersAPI.getAll()
    producers.value = res.data.data
  } catch (e) {
    error.value = e.response?.data?.message || 'Failed to load'; producers.value = []
  } finally { loading.value = false }
}

function openAdd() { editTarget.value = null; showModal.value = true }
function openEdit(p) { editTarget.value = p; showModal.value = true }
function confirmDelete(p) { deleteTarget.value = p }

async function handleSave(data) {
  try {
    if (editTarget.value) await producersAPI.update(editTarget.value.id, data)
    else await producersAPI.create(data)
    showModal.value = false
    await load()
  } catch (e) { alert(e.response?.data?.message || 'Save failed') }
}

async function doDelete() {
  deleting.value = true
  try {
    await producersAPI.delete(deleteTarget.value.id)
    producers.value = producers.value.filter(p => p.id !== deleteTarget.value.id)
    deleteTarget.value = null
  } catch (e) { alert(e.response?.data?.message || 'Delete failed') }
  finally { deleting.value = false }
}

onMounted(load)
</script>

<style scoped>
.avatar-circle {
  width: 42px; height: 42px; border-radius: 50%;
  color: #fff; display: flex; align-items: center; justify-content: center;
  font-weight: bold; font-size: 1.1rem; flex-shrink: 0;
}
.modal-backdrop-custom {
  position: fixed; inset: 0; background: rgba(0,0,0,0.4);
  display: flex; align-items: center; justify-content: center; z-index: 9999;
}
.modal-box {
  background: #fff; border-radius: 10px; padding: 24px 28px;
  width: 360px; box-shadow: 0 8px 30px rgba(0,0,0,0.2);
}
</style>
