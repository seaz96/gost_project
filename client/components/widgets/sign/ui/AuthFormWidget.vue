<template>
<div class="registrationForm">
    <img src="/img/urfu-logo.png" class="registrationForm__logo">
    <h2 class="registrationForm__title">Авторизация</h2>
    <DynamicForm
      v-model:errors="errors"
      v-model:model-value="formData"
      :form-fields="formFields"
      :modifier="modifier"
    >
      <template #submitButton>
        <div class="registrationForm__buttons">
          <BaseButton
            class="registrationForm__button"
            text="Войти"
            :is-colored="true"
            :action="() => {
              login()
            }"
          />
          <BaseButton
            class="registrationForm__button"
            text="Зарегистрироваться"
            :is-colored="false"
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
import { UserApi } from '~/components/features/user/api/UserApi';
import { BackendErrorParser } from '~/components/shared/parsers/BackendErrorParser';
import { useUserStore } from '~/components/entities/user';

const formData = ref<Record<string, string>>({})
const errors = ref<Record<string, string>>({})
const userApi = UserApi()
const userStore = useUserStore()

const activeForm = defineModel<'auth' | 'registration'>('activeForm')

const login = async () => {
  errors.value = {}
  try {
    const user = await userApi.login(
      {
        login: formData.value.login,
        password: formData.value.password
      }
    )
      
    if(user) {
      userStore.currentUser = user
      localStorage.setItem('gost-storage-token', user.token)
      navigateTo('/')
    }
  } catch (err: unknown) {
    for (const field of Object.keys(formData.value)) {
      if(BackendErrorParser(err)?.includes(field)) {
        errors.value[field] = "Неверный ввод"
      }
    }
  }
}

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