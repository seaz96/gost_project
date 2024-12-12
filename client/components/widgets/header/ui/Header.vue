<script setup lang="ts">
import BaseButton from '~/components/shared/ui/form/BaseButton.vue';
</script>

<template>
  <div class="main-header">
    <div class="main-header__buttons">
      <BaseButton
        text="Все документы"
        :is-colored="false"
      />
      <BaseButton
        text="Создать документ"
        :is-colored="false"
        :prefix-icon="'/img/plus.png'"
      />
    </div>
    <ul class="main-header__list">
      <li class="main-header__list-item">
        <NuxtLink to="/users">Пользователи</NuxtLink>
      </li>
      <li class="main-header__list-item">
        <NuxtLink to="/archive">Архив</NuxtLink>
      </li>
      <li class="main-header__list-item">
        <NuxtLink to="/statistic">Статистика</NuxtLink>
      </li>
    </ul>
    <BaseButton
        text="Профиль"
        :is-colored="false"
        :prefix-icon="'/img/profile.png'"
        :suffix-icon="'/img/chevron-down.png'"
      />
  </div>
</template>

<style lang="scss">
.main-header {
  display: flex;
  padding: 16px 20px;
  background: linear-gradient(180deg, #CB0E70 -26%, #F1B72C 183%);

  &__buttons {
    display: flex;
    gap: 10px;
  }

  &__list {
    display: flex;
    flex-grow: 1;
    justify-content: space-around;
    align-items: center;

    &-item a {
      color: #fff;
      font-family: "Manrope", Arial, Helvetica, sans-serif;
      font-size: 17px;
      line-height: 20px;
      font-weight: 700;
    }
  }
}
</style>