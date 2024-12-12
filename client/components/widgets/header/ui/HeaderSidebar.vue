<template>
  <div class="header-sidebar">
    <ul class="header-sidebar__list">
      <li class="header-sidebar__list-item">
        <NuxtLink to="/users">Все документы</NuxtLink>
      </li>
      <li class="header-sidebar__list-item">
        <NuxtLink to="/users">Создать документ</NuxtLink>
      </li>
      <li class="header-sidebar__list-item">
        <NuxtLink to="/users">Пользователи</NuxtLink>
      </li>
      <li class="header-sidebar__list-item">
        <NuxtLink to="/archive">Архив</NuxtLink>
      </li>
      <li class="header-sidebar__list-item">
        <NuxtLink to="/statistic">Статистика</NuxtLink>
      </li>
    </ul>
  </div>
</template>

<style lang="scss">
.header-sidebar {
  width: 100%;
  height: 100%;
  padding: 16px 20px;
  background: linear-gradient(180deg, #CB0E70 -26%, #F1B72C 183%);

  &__list {
    display: flex;
    flex-direction: column;
    flex-grow: 1;
    gap: 20px;
    margin-top: 50px;
    margin-bottom: 50px;
    justify-content: space-around;
    align-items: start;

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