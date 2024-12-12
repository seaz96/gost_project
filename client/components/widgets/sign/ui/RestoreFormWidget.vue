<template>
  <DynamicForm
      v-model:errors="errors"
      v-model:model-value="formData"
      :form-fields="formFields"
      :modifier="modifier"
    >
      <template #submitButton>
        <BaseButton
          text="Сменить пароль"
          :is-colored="true"
        />
      </template> 
    </DynamicForm>
</template>

<script setup lang="ts">
import DynamicForm from '~/components/entities/form/ui/DynamicForm.vue';
import type { formField } from '~/components/shared/types/formField.type';
import BaseButton from '~/components/shared/ui/form/BaseButton.vue';

const formData = ref<Record<string, string>>({})
const errors = ref<Record<string, string>>({})

const formFields: formField[] = [
  {
    label: 'Старый пароль',
    name: 'old_password',
    type: 'text'
  },
  {
    label: 'Новый пароль',
    name: 'new_password',
    type: 'password'
  },
  {
    label: 'Повторите новый пароль',
    name: 'new_password_repeat',
    type: 'password'
  },
]

const modifier = {
  form: {
    width: '100%',
    maxWidth: '430px',
    display: 'flex',
    flexDirection: 'column',
    gap: '14px'
  }
}
</script>