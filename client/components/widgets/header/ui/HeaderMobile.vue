<template>
  <div class="mobile-header">
    <button class="mobile-header__sidebar-button" @click="isOpen = !isOpen">
      <span />
    </button>
    <BaseButton
        text="Профиль"
        :is-colored="false"
        :prefix-icon="'/img/profile.png'"
        :suffix-icon="'/img/chevron-down.png'"
      />
  </div>
</template>

<script setup lang="ts">
import BaseButton from '~/components/shared/ui/form/BaseButton.vue';

const isOpen = defineModel<boolean>("sidebar")
</script>

<style lang="scss">
.mobile-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: linear-gradient(180deg, #CB0E70 -26%, #F1B72C 183%);
  padding: 16px 20px;

  &__sidebar-button {
    width: 40px;
    height: 35px;
    background-color: #fff;
    border: none;
    border-radius: 5px;

    span {
      display: block;
      width: 100%;
      height: 3px;
      border-radius: 3px;
      background: linear-gradient(180deg, #CB0E70 -26%, #F1B72C 183%);
    }

    &::after {
      content: "";

      display: block;
      margin-top: 5px;
      width: 100%;
      height: 3px;
      border-radius: 3px;
      background: linear-gradient(180deg, #CB0E70 -26%, #F1B72C 183%);
    }

    &::before {
      content: "";

      display: block;
      margin-bottom: 5px;
      width: 100%;
      height: 3px;
      border-radius: 3px;
      background: linear-gradient(180deg, #CB0E70 -26%, #F1B72C 183%);
    }
  }
}
</style>