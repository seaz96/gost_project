<template>
  <form class="baseForm" :style="modifierFieldToString(modifier?.form || {})">
    <template v-for="field in formFields" :key="field.name">
      <BaseInput
        v-model:model-value="formData[field.name]"
        :label="field.label"
        :type="field.type"
        :error="errors[field.name]"
        :style="
          modifierFieldToString(modifier?.inputs || {}) + 
          modifierFieldToString(modifier?.[field.name] || {})
        "
        :input-change="() => {clearError(field.name)}"
      />
    </template>
    <slot name="submitButton" />
  </form>
</template>

<script setup lang="ts">
import type { formField } from '~/components/shared/types/formField.type';
import BaseInput from '~/components/shared/ui/form/BaseInput.vue';

defineProps<{
  formFields: formField[],
  modifier?: Record<string, object>
}>()

const clearError = (field: string) => {
   errors.value[field] = ""
}

const modifierFieldToString = (modifierField: object) => {
  let str = ""
  for (const [key, value] of Object.entries(modifierField)) {
    const newKey = key.replace(/[A-Z]/g, (letter: string) => `-${letter.toLowerCase()}`);
    str += `${newKey}: ${value}; `
  }
  return str
}

const formData = defineModel<Record<string, string>>({ default: {} })
const errors = defineModel<Record<string, string>>('errors', { default: {} })
</script>