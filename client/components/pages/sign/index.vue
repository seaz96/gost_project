<template>
  <AuthFormWidget 
    v-model:active-form="activeForm" 
    v-if="activeForm === 'auth'"
  />
  <RegistrationFormWidget
    v-model:active-form="activeForm" 
    v-if="activeForm === 'registration'"
  />
</template>

<script setup lang="ts">
import AuthFormWidget from '~/components/widgets/sign/ui/AuthFormWidget.vue';
import RegistrationFormWidget from '~/components/widgets/sign/ui/RegistrationFormWidget.vue';

const activeForm = ref<'auth' | 'registration'>('auth')
</script>