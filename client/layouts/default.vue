<template>
  <div class="main-layout">
    <div v-if="isOpen" class="main-overlay" @click="isOpen = false"/>

    <header class="main-layout__mobile-header">
      <HeaderMobile v-model:sidebar="isOpen"/>
    </header>

    <header class="main-layout__header">
      <Header />
    </header>

    <main class="container">
      <slot />
    </main>

    <div :class="['main-layout__header-sidebar', isOpen && 'main-layout__header-sidebar-open']">
      <HeaderSidebar />
    </div>
  </div>
</template>

<script setup lang="ts">
import Header from '~/components/widgets/header/ui/Header.vue';
import HeaderMobile from '~/components/widgets/header/ui/HeaderMobile.vue';
import HeaderSidebar from '~/components/widgets/header/ui/HeaderSidebar.vue';

const isOpen = ref(false)
</script>

<style lang="scss">
.main-blocked {
  overflow: hidden;
  background-color: rgba(0, 0, 0, 0.6);
}

.main-layout {
  min-height: 100vh;

  &__header-sidebar {
    position: absolute;
    top: 0;
    left: -100%;
    width: 33%;
    height: 100vh;
    max-width: 300px;
    min-width: 200px;
    transition: all 0.3s ease-in;
    z-index: 200;

    &-open {
      left: 0
    }
  }

  &__header {
    display: block;
  }

  &__mobile-header {
    display: none;
  }
}

@media(max-width: 850px) {
  .main-layout__header {
    display: none;
  }

  .main-layout__mobile-header {
    display: block;
  }
}

.main-overlay {
  position: absolute;
  height: 100vh;
  width: 100vw;
  z-index: 100;
  background-color: rgba(0, 0, 0, 0.6);
}
</style>