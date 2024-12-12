
<template>
  <div class="registrationForm">
    <img src="/img/urfu-logo.png" class="registrationForm__logo">
    <h2 class="registrationForm__title">Регистрация</h2>
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
            text="Зарегистрироваться"
            :is-colored="true"
            :action="() => {
              register()
            }"
          />
          <BaseButton
            class="registrationForm__button"
            text="Авторизоваться"
            :is-colored="false"
            :action="() => {
              activeForm = 'auth'
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
import { useUserStore } from '~/components/entities/user';
import { BackendErrorParser } from '~/components/shared/parsers/BackendErrorParser';

const formData = ref<Record<string, string>>({})
const errors = ref<Record<string, string>>({})
const userApi = UserApi()
const userStore = useUserStore()

const activeForm = defineModel<'auth' | 'registration'>('activeForm')

const register = async () => {
  errors.value = {}

  if(formData.value.password !== formData.value.passwordConfirm) {
    errors.value.passwordConfirm = "Пароли не совпадают"
    return
  }

  try {
    const user = await userApi.register(
      {
        login: formData.value.login,
        name: formData.value.fullname,
        password: formData.value.password,
        orgName: formData.value.organizationName,
        orgBranch: formData.value.organizationDepartment,
        orgActivity: formData.value.organizationnActivity,
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
  {
    label: 'Подтвердите пароль',
    name: 'passwordConfirm',
    type: 'password'
  },
  {
    label: 'ФИО пользователя',
    name: 'fullname',
    type: 'text'
  },
  {
    label: 'Название организации',
    name: 'organizationName',
    type: 'text'
  },
  {
    label: 'Отделение организации',
    name: 'organizationDepartment',
    type: 'text'
  },
  {
    label: 'Деятельность организации',
    name: 'organizationnActivity',
    type: 'text'
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

<style lang="scss">
.registrationForm {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding-bottom: 40px;

  &__logo {
    margin-bottom: 40px;
  }

  &__title {
    font-family: "Manrope";
    font-size: 30px;
    font-weight: 400;
    line-height: 40px;
    text-align: center;
    margin-bottom: 16px;
  }

  &__buttons {
    width: 100%;
    display: flex;
    flex-wrap: wrap;
    margin-top: 2px;
    gap: 16px;  
  }

  &__button {
    flex-grow: 1;
  }
}
</style>