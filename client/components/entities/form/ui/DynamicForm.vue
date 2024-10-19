<template>
  <form class="baseForm" :style="modifierFieldToString(modifier?.form || {})">
    <template v-for="field in formFields" :key="field.name">
      <BaseInput
        :label="field.label"
        :type="field.type"
        :error="errors[field.name]"
        :style="
          modifierFieldToString(modifier?.inputs || {}) + 
          modifierFieldToString(modifier?.[field.name] || {})
        "
        v-model:model-value="formData[field.name]"
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
  modifier?: Record<string, Object>
}>()

const modifierFieldToString = (modifierField: Object) => {
  let str = ""
  for (const [key, value] of Object.entries(modifierField)) {
    const newKey = key.replace(/[A-Z]/g, (letter: string) => `-${letter.toLowerCase()}`);
    console.log(newKey)
    str += `${newKey}: ${value}; `
  }
  return str
}

const formData = defineModel<Record<string, string>>({ default: {} })
const errors = defineModel<Record<string, string>>('errors', { default: {} })
</script>