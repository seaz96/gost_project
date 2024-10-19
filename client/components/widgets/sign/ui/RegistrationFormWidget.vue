
<template>
  <div class="registrationForm">
    <img src="/img/urfu-logo.png" class="registrationForm__logo">
    <h2 class="registrationForm__title">Регистрация</h2>
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
            text="Зарегистрироваться"
            :isColored="true"
            :action="() => {
              errors.email = 'СОСИ ЖОПУ'
            }"
          />
          <BaseButton
            class="registrationForm__button"
            text="Авторизоваться"
            :isColored="false"
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
  margin-bottom: 40px;

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