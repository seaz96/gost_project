<template>
<div class="registrationForm">
    <img src="/img/urfu-logo.png" class="registrationForm__logo">
    <h2 class="registrationForm__title">Авторизация</h2>
    <DynamicForm
      :form-fields="formFields"
      :modifier="modifier"
      v-model:errors="errors"
      v-model:model-value="formData"
    >
      <template #submitButton>
        <div class="registrationForm__buttons">
          <BaseButton
            class="registrationForm__button"
            text="Войти"
            :isColored="true"
            :action="() => {
              errors.email = 'СОСИ ЖОПУ'
            }"
          />
          <BaseButton
            class="registrationForm__button"
            text="Зарегистрироваться"
            :isColored="false"
            :action="() => {
              activeForm = 'registration'
            }"
          />
        </div>
      </template> 
    </DynamicForm>
  </div>
</template>

<script setup lang="ts">
import DynamicForm from '~/components/entities/form/ui/DynamicForm.vue';
import BaseButton from '~/components/shared/ui/form/BaseButton.vue';
import type { formField } from '~/components/shared/types/formField.type';

const formData = ref<Record<string, string>>({})
const errors = ref<Record<string, string>>({})

const activeForm = defineModel<'auth' | 'registration'>('activeForm')

const formFields: formField[] = [
  {
    label: 'Логин',
    name: 'login',
    type: 'text'
  },
  {
    label: 'Пароль',
    name: 'password',
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