<template>
  <NConfigProvider>
    <NModalProvider>
      <NuxtLayout>
        <NuxtPage />
      </NuxtLayout>
    </NModalProvider>
  </NConfigProvider>
</template>

<script setup lang="ts">
import { NConfigProvider, NModalProvider } from 'naive-ui'
</script>
