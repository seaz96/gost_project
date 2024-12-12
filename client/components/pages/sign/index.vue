<template>
  <div>
    <AuthFormWidget 
      v-if="activeForm === 'auth'" 
      v-model:active-form="activeForm"
    />
    <RegistrationFormWidget
      v-if="activeForm === 'registration'" 
      v-model:active-form="activeForm"
    />
  </div>
</template>

<script setup lang="ts">
import AuthFormWidget from '~/components/widgets/sign/ui/AuthFormWidget.vue';
import RegistrationFormWidget from '~/components/widgets/sign/ui/RegistrationFormWidget.vue';

const activeForm = ref<'auth' | 'registration'>('auth')

definePageMeta({
  layout: 'empty'
})
</script>