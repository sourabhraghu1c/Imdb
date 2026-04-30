<template>
  <div class="modal-backdrop-custom" @click.self="$emit('close')">
    <div class="modal-box">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h6 class="fw-bold mb-0">{{ isEdit ? `Edit ${title}` : `Add ${title}` }}</h6>
        <button class="btn-close" @click="$emit('close')"></button>
      </div>

      <div v-if="error" class="alert alert-danger small py-2">{{ error }}</div>

      <div class="mb-3">
        <label class="form-label small fw-semibold">Name</label>
        <input v-model="form.name" type="text" class="form-control bg-light border-0" placeholder="Full name" />
      </div>
      <div class="mb-3">
        <label class="form-label small fw-semibold">Bio</label>
        <textarea v-model="form.bio" rows="3" class="form-control bg-light border-0" placeholder="Short biography"></textarea>
      </div>
      <div class="mb-3">
        <label class="form-label small fw-semibold">Date of Birth</label>
        <input v-model="form.dob" type="date" class="form-control bg-light border-0" />
      </div>
      <div class="mb-4">
        <label class="form-label small fw-semibold">Gender</label>
        <select v-model="form.gender" class="form-select bg-light border-0">
          <option value="">Select gender</option>
          <option value="M">Male (M)</option>
          <option value="F">Female (F)</option>
        </select>
      </div>

      <div class="d-flex gap-2 justify-content-end">
        <button class="btn btn-sm btn-outline-secondary" @click="$emit('close')">Cancel</button>
        <button class="btn btn-sm btn-primary" :disabled="submitting" @click="submit">
          <span v-if="submitting" class="spinner-border spinner-border-sm me-1"></span>
          {{ isEdit ? 'Update' : 'Add' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, watch } from 'vue'

const props = defineProps({
  title: String,
  editData: Object   // pass null for add, object for edit
})
const emit = defineEmits(['close', 'saved'])

const isEdit = !!props.editData
const error = ref('')
const submitting = ref(false)

const form = reactive({
  name: props.editData?.name || '',
  bio: props.editData?.bio || '',
  dob: props.editData?.dob ? props.editData.dob.substring(0, 10) : '',
  gender: props.editData?.gender || ''
})

async function submit() {
  error.value = ''
  if (!form.name || !form.bio || !form.dob || !form.gender) {
    error.value = 'All fields are required'
    return
  }
  submitting.value = true
  try {
    emit('saved', {
      name: form.name,
      bio: form.bio,
      dob: new Date(form.dob).toISOString(),
      gender: form.gender
    })
  } finally {
    submitting.value = false
  }
}
</script>

<style scoped>
.modal-backdrop-custom {
  position: fixed; inset: 0;
  background: rgba(0,0,0,0.4);
  display: flex; align-items: center; justify-content: center;
  z-index: 9999;
}
.modal-box {
  background: #fff;
  border-radius: 12px;
  padding: 28px;
  width: 440px;
  box-shadow: 0 8px 30px rgba(0,0,0,0.18);
}
</style>
