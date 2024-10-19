<template>
  <div class="baseInput">
    <p class="baseInput__label">
      {{ label }}
    </p>
    <input
      v-model="value"
      :type="type"
      :class="['baseInput__input', error && error.length && 'baseInput__input_error']"
    >
    <p
      class="baseInput__error"
    >
      {{ error }}
    </p>
  </div>
</template>

<script setup lang="ts">
  const value = defineModel()

  withDefaults(defineProps<{
    type?: string,
    error?: string,
    label?: string
  }>(), {
    type:'text',
    error: '',
    label: ''
  })
</script>

<style lang="scss">
  .baseInput {
    font-family: "Manrope", Arial, Helvetica, sans-serif;
    font-size: 15px;
    font-weight: 400;
    line-height: 20px;


    &__input {
      width: 100%;
      border: 1px solid #D2D2D2;
      padding: 14px 20px 14px 20px;
      border-radius: 5px;

      &_error {
        border-color: var(--color-error);
      }
    }

    &__error {
      color: var(--color-error);
      font-size: 14px;
      margin-left: 8px;
    }

    &__label {
      margin-bottom: 8px;
      font-weight: 500;
      font-size: 16px;
    }
  }
</style> 